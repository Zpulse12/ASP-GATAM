#!/bin/sh


# Opzetten van nieuwe machine / server. OPGELET! Dit script gaat ook een GITLAB runner starten voor de pipelines te bouwen.
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
        sudo systemctl start $package
        sudo systemctl enable $package
    else
        echo "$package already installed, skipping..."
    fi
}

install_gitlab_runner() {
    package=$1
    sudo apt install -y $package
     if ! command -v "$package" >/dev/null 2>&1; then
        echo "$package not found. Installing..."
        echo "Installing gitlab repositories"
        curl -L https://packages.gitlab.com/install/repositories/runner/gitlab-runner/script.deb.sh | sudo bash
        echo "Installing $package"
        sudo apt update
        sudo apt install -y gitlab-runner
        echo "Setting permissions for GITLAB RUNNER to access ENV file."
        setfacl -m u:gitlab-runner:r /etc/gatam/.env.production
        setfacl -m u:gitlab-runner:r /etc/gatam/.env.staging
        echo "Prompting registration. Please fill in the right data"
        sudo gitlab-runner register
        sudo systemctl enable gitlab-runner
        sudo systemctl start gitlab-runner
    else
        echo "$package already installed, skipping..."
        if ! sudo gitlab-runner list | grep -q "ID:"; then
            echo "Prompting registration. Please fill in the right data"
            sudo gitlab-runner register
            sudo systemctl enable gitlab-runner
            sudo systemctl start gitlab-runner
        else
            echo "GitLab Runner is already registered."
        fi
    fi
}


echo "Creating necessary files :"
mkdir /etc/gatam
cd /etc/gatam
touch .env.production
touch .env.staging
cd /
install_if_missing git
install_docker_if_missing docker 
install_gitlab_runner gitlab-runner


