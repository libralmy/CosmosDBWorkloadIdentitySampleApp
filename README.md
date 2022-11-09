### Azure CosmosDBManagement client library for dotnet helps you to perform the Cosmos DB Managemnt operation mentioned [here](https://azuresdkdocs.blob.core.windows.net/$web/dotnet/Azure.ResourceManager.CosmosDB/1.0.0-preview.1/api/Azure.ResourceManager.CosmosDB/Azure.ResourceManager.CosmosDB.CosmosDBManagementClient.html)

# Prerequisites

You need an Azure subscription to run these sample programs.
1. Azure Cosmos DB account
2. Azure Managed Identity for Cosmos DB 

### Create Kubernetes service account [here](https://learn.microsoft.com/en-us/azure/aks/learn/tutorial-kubernetes-workload-identity)
Create a Kubernetes service account and annotate it with the client ID of the Managed Identity
```sh
```
### Build the Image
Creating Docker Images from .NET Core Applications is straight forward. Microsoft provides all the required Base-Images to make your application run in a Linux-based container. We are going to create a so-called multi-stage Docker Image.

```sh
docker build -t cosmosdb-aks-app:0.0.1 -f Dockerfile.txt .
```

### Push The Docker Image To ACR
```sh
docker tag cosmos-aks-app:0.0.1 $(ACR_NAME).azurecr.io/cosmos-aks-app:0.0.1
```

Authenticate against ACR
```sh
az acr login -n $(ACR_NAME)
```
### Create Kubernetes Deployment Manifests 
Create a yaml file
```sh
## pod.yml
apiVersion: v1
kind: Pod
metadata:
  name: cosmos-aks-app
  labels:
    app: cosmosdb-aks-env
    component: netcore-app
  namespace: default
spec:
  serviceAccountName: cosmosdbserviceaccountname
  containers:
    - image: acremily3.azurecr.io/cosmosdb-aks-env:0.0.1
      name: cosmosdb-aks-env
      ports:
      - containerPort: 80
  nodeSelector:
    kubernetes.io/os: linux
```
### Deploy the Application
```bash
kubectl apply -f pod.yml
## result
pod/cosmos-aks-app created
```
You can check your Pod using
```sh
kubectl get pods
## result
NAME             READY   STATUS    RESTARTS   AGE
cosmos-aks-app   1/1     Running   0          7s
```# CosmosDB-WorkloadIdentity-SampleApp 
