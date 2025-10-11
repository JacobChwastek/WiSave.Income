#!/bin/bash

# Docker Cleanup Script for Mac
# This script removes all Docker containers, images, volumes, and networks

echo "🐳 Docker Cleanup Script"
echo "========================"
echo ""
echo "⚠️  WARNING: This will remove:"
echo "   - All containers (running and stopped)"
echo "   - All images"
echo "   - All volumes"
echo "   - All networks (except default ones)"
echo ""

read -p "Are you sure you want to continue? (yes/no): " confirmation

if [ "$confirmation" != "yes" ]; then
    echo "Cleanup cancelled."
    exit 0
fi

echo ""
echo "Starting cleanup..."
echo ""

# Stop all running containers
echo "🛑 Stopping all running containers..."
docker stop $(docker ps -aq) 2>/dev/null
if [ $? -eq 0 ]; then
    echo "✅ Containers stopped"
else
    echo "ℹ️  No running containers to stop"
fi
echo ""

# Remove all containers
echo "🗑️  Removing all containers..."
docker rm $(docker ps -aq) 2>/dev/null
if [ $? -eq 0 ]; then
    echo "✅ Containers removed"
else
    echo "ℹ️  No containers to remove"
fi
echo ""

# Remove all images
echo "🗑️  Removing all images..."
docker rmi $(docker images -q) -f 2>/dev/null
if [ $? -eq 0 ]; then
    echo "✅ Images removed"
else
    echo "ℹ️  No images to remove"
fi
echo ""

# Remove all volumes
echo "🗑️  Removing all volumes..."
docker volume rm $(docker volume ls -q) 2>/dev/null
if [ $? -eq 0 ]; then
    echo "✅ Volumes removed"
else
    echo "ℹ️  No volumes to remove"
fi
echo ""

# Remove all networks (except default ones)
echo "🗑️  Removing all custom networks..."
docker network prune -f 2>/dev/null
echo "✅ Networks cleaned"
echo ""

# Run system prune to clean up everything else
echo "🧹 Running final system prune..."
docker system prune -a -f --volumes
echo ""

echo "✨ Docker cleanup complete!"
echo ""

# Show current Docker disk usage
echo "Current Docker disk usage:"
docker system df