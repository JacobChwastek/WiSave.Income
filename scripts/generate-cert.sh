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
COMMON_NAME="localhost"

# Colors for output
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

echo -e "${GREEN}=== WiSave Income WebApi Certificate Generator ===${NC}\n"

# Create certs directory if it doesn't exist
mkdir -p "$CERT_DIR"

# Create OpenSSL config file with proper extensions
cat > "$CERT_DIR/openssl.cnf" << EOF
[req]
default_bits = 2048
prompt = no
default_md = sha256
distinguished_name = dn
x509_extensions = v3_req

[dn]
C = $COUNTRY
ST = $STATE
L = $CITY
O = $ORG
OU = $ORG_UNIT
CN = $COMMON_NAME

[v3_req]
subjectAltName = @alt_names
basicConstraints = CA:FALSE
keyUsage = critical, digitalSignature, keyEncipherment
extendedKeyUsage = serverAuth

[alt_names]
DNS.1 = localhost
DNS.2 = *.localhost
DNS.3 = wisave.income.webapi
DNS.4 = 127.0.0.1
IP.1 = 127.0.0.1
IP.2 = 0.0.0.0
EOF

# Generate private key and self-signed certificate in one step
echo -e "${YELLOW}Generating private key and certificate...${NC}"
openssl req -x509 -new -nodes \
    -newkey rsa:2048 \
    -keyout "$CERT_DIR/$CERT_NAME.key" \
    -out "$CERT_DIR/$CERT_NAME.crt" \
    -days $DAYS_VALID \
    -config "$CERT_DIR/openssl.cnf" \
    -extensions v3_req

# Generate PFX file for .NET
echo -e "${YELLOW}Generating PFX file for .NET...${NC}"
openssl pkcs12 -export -out "$CERT_DIR/$CERT_NAME.pfx" \
    -inkey "$CERT_DIR/$CERT_NAME.key" \
    -in "$CERT_DIR/$CERT_NAME.crt" \
    -passout pass:wisave123

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

# Verify certificate
echo -e "\n${YELLOW}Verifying certificate...${NC}"
openssl x509 -in "$CERT_DIR/$CERT_NAME.crt" -noout -text | grep -A 1 "Key Usage"
openssl x509 -in "$CERT_DIR/$CERT_NAME.crt" -noout -text | grep -A 1 "Extended Key Usage"
openssl x509 -in "$CERT_DIR/$CERT_NAME.crt" -noout -text | grep -A 5 "Subject Alternative Name"

echo -e "\n${GREEN}✅ Ready to use!${NC}"
echo "The certificate is at the solution root in ./certs/"
echo ""
echo "To trust this certificate on macOS:"
echo "  sudo security add-trusted-cert -d -r trustRoot -k /Library/Keychains/System.keychain $CERT_DIR/$CERT_NAME.crt"
echo ""