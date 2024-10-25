#!/bin/sh


# Used to setup a new VM or Dedicated Machine. Make sure every dependency is installed.
install_if_missing() {
    package=$1

    if ! command -v "$package" >/dev/null 2>&1; then
        echo "$package not found. Installing..."
        sudo apt update
        sudo apt install -y "$package"
        echo "$package has been installed."
    else
        echo "$package already installed, skipping..."
    fi
}
install_docker_if_missing() {
    package=$1
    if ! command -v "$package" >/dev/null 2>&1; then
        echo "$package not found. Installing..."
        sudo apt update
        sudo apt install -y ca-certificates curl gnupg lsb-release
        curl -fsSL https://download.docker.com/linux/debian/gpg | sudo gpg --dearmor -o /usr/share/keyrings/docker-archive-keyring.gpg
        echo "deb [arch=$(dpkg --print-architecture) signed-by=/usr/share/keyrings/docker-archive-keyring.gpg] https://download.docker.com/linux/debian bullseye stable" | sudo tee /etc/apt/sources.list.d/docker.list > /dev/null
        sudo apt update
        sudo apt install -y docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin
        echo "$package has been installed."
        echo "Setting $package to start on startup + starting the deamon"
        sudo systemctl start docker
        sudo systemctl enable docker
    else
        echo "$package already installed, skipping..."
    fi
}

install_if_missing git
install_docker_if_missing docker 



