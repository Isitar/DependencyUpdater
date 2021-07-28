# Isitar Dependency Updater

## ⚠️ This is a work in progress project ⚠️

## Basic functionality

- Manage Projects and collaboration platforms
- Create Merge-Requests / Pull-Requests with updates for outdated dependencies
- **Works with private/self hosted gitlabs**

## Install in K8s

### Required Environment Variables

| Name | Description | SuggestedValue |
| --- | --- | --- |
|DatabaseSettings__Location | The file location for the sqlite db | /usr/local/share/dependency-updater/ |

### Ports
The port **80** is exposed for incoming http requests. **443** is not exposed, since normally ssl is terminated on the ingress 