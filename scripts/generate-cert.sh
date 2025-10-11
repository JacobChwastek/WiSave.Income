#!/bin/bash

# Get the directory where the script is located
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
# Go up one level to solution root
SOLUTION_ROOT="$(dirname "$SCRIPT_DIR")"
CERT_DIR="$SOLUTION_ROOT/certs"

CERT_NAME="wisave-income-webapi"
DAYS_VALID=365
COUNTRY="PL"
STATE="Mazovia"
CITY="Warsaw"
ORG="WiSave"
ORG_UNIT="Income"
COMMON_NAME="wisave.income.webapi"

# Colors for output
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

echo -e "${GREEN}=== WiSave Income WebApi Certificate Generator ===${NC}\n"

# Create certs directory if it doesn't exist
mkdir -p "$CERT_DIR"

# Generate private key
echo -e "${YELLOW}Generating private key...${NC}"
openssl genrsa -out "$CERT_DIR/$CERT_NAME.key" 2048

# Generate certificate signing request (CSR)
echo -e "${YELLOW}Generating certificate signing request...${NC}"
openssl req -new -key "$CERT_DIR/$CERT_NAME.key" \
    -out "$CERT_DIR/$CERT_NAME.csr" \
    -subj "/C=$COUNTRY/ST=$STATE/L=$CITY/O=$ORG/OU=$ORG_UNIT/CN=$COMMON_NAME"

# Create config file for SAN (Subject Alternative Names)
cat > "$CERT_DIR/san.cnf" << EOF
[req]
default_bits = 2048
distinguished_name = req_distinguished_name
req_extensions = v3_req
prompt = no

[req_distinguished_name]
C = $COUNTRY
ST = $STATE
L = $CITY
O = $ORG
OU = $ORG_UNIT
CN = $COMMON_NAME

[v3_req]
keyUsage = keyEncipherment, dataEncipherment
extendedKeyUsage = serverAuth
subjectAltName = @alt_names

[alt_names]
DNS.1 = localhost
DNS.2 = wisave.income.webapi
DNS.3 = *.wisave.income.webapi
IP.1 = 127.0.0.1
IP.2 = 0.0.0.0
EOF

# Generate self-signed certificate
echo -e "${YELLOW}Generating self-signed certificate...${NC}"
openssl x509 -req -days $DAYS_VALID \
    -in "$CERT_DIR/$CERT_NAME.csr" \
    -signkey "$CERT_DIR/$CERT_NAME.key" \
    -out "$CERT_DIR/$CERT_NAME.crt" \
    -extensions v3_req \
    -extfile "$CERT_DIR/san.cnf"

# Generate PFX file for .NET
echo -e "${YELLOW}Generating PFX file for .NET...${NC}"
openssl pkcs12 -export -out "$CERT_DIR/$CERT_NAME.pfx" \
    -inkey "$CERT_DIR/$CERT_NAME.key" \
    -in "$CERT_DIR/$CERT_NAME.crt" \
    -password pass:wisave123

# Set permissions
chmod 644 "$CERT_DIR/$CERT_NAME.crt"
chmod 600 "$CERT_DIR/$CERT_NAME.key"
chmod 644 "$CERT_DIR/$CERT_NAME.pfx"

# Display certificate info
echo -e "\n${GREEN}✓ Certificate generated successfully!${NC}\n"
echo "📁 Location: $CERT_DIR"
echo ""
echo "Certificate files created:"
echo "  📄 Private Key: $CERT_NAME.key"
echo "  📄 Certificate: $CERT_NAME.crt"
echo "  📄 PFX (for .NET): $CERT_NAME.pfx"
echo "  🔑 Password: wisave123"
echo ""
echo "📅 Valid for: $DAYS_VALID days"
echo "🌐 Common Name: $COMMON_NAME"

# Display certificate details
echo -e "\n${YELLOW}Certificate Details:${NC}"
openssl x509 -in "$CERT_DIR/$CERT_NAME.crt" -noout -text | grep -A 2 "Subject:"
openssl x509 -in "$CERT_DIR/$CERT_NAME.crt" -noout -text | grep -A 3 "Subject Alternative Name"

echo -e "\n${GREEN}✅ Ready to use!${NC}"
echo "The certificate is at the solution root in ./certs/"
echo ""