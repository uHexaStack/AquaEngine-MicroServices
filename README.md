## Before starting

### This micro services uses consul server to replace the 

Run this line in the terminal and if you already have the image, make sure it runs.

```bash
docker run -d --name=consul -p 8500:8500 hashicorp/consul:latest
```