# create 2 WebApis
1. PlatformService
2. CommandService

# In PlatformService WebApi
dotnet add AutoMapper.Extensions.Microsoft.DependencyInjection
dotnet add Microsoft.EntityFrameworkCore
dotnet add Microsoft.EntityFrameworkCore.Design
dotnet add Microsoft.EntityFrameworkCore.InMemory
dotnet add Microsoft.EntityFrameworkCore.sqlserver

setup PlatformController, models, dtos, automapper profile, AppDbContext, Initial data seeding, IPlatform Repository


# Docker In Action

docker build -t devxbasit/platformservice .
docker push devxbasit/platformservice

docker build -t devxbasit/commandservice .
docker push devxbasit/commandservice

docker run -p 127.0.0.1:8080:8080 -d devxbasit/platformservice
docker run -p 127.0.0.1:8081:8080 -d devxbasit/commandservice

docker ps
docker stop <container Id>
docker start <container Id>

http://localhost:8080/api/platform
http://localhost:8081/api/c/platform


# Kubernetes In Action
kubectl apply -f platforms-depl.yaml
kubectl apply -f platforms-np-srv.yaml
kubectl apply -f commands-depl.yaml
kubectl apply -f commands-np-srv.yaml

kubectl rollout restart deployment platforms-depl
kubectl rollout restart deployment commands-depl

kubectl delete deployment platforms-depl
kubectl delete service platformnpservice-srv
kubectl delete deployment commands-depl


kubectl version
kubectl get deployments
kubectl get pods
kubectl get pods --show-labels
kubectl get pods --output=wide
kubectl get services
kubectl get all
kubectl apply -f < yaml file name>
kubectl delete deployment <deployment name>
kubectl rollout restart deployment <deployment name>
kubectl delete deployment --all
kubectl delete services --all
kubectl delete deployment --all --namespace=ingress-nginx
kubectl delete services --all --namespace=ingress-nginx
kubectl delete pvc --all

# In CommandsService WebApi
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.InMemory
dotnet add package Automapper.Extensions.Microsoft.DependencyInjection

# API Gateway ingress nginx
https://kubernetes.github.io/ingress-nginx/deploy/#docker-desktop

kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.12.0/deploy/static/provider/cloud/deploy.yaml
kubectl apply -f ingress-srv.yaml

kubectl get pods --namespace=ingress-nginx
kubectl get namespace

there is some issue with the ingress-nginx
I have set  "- path: /" instead of  "- path: /api/platform" in ingress-srv.yaml - (some have recommended to use app.UsePathBase("/api");)

# Configuring SQL Server in Both API
kubectl create secret generic platforms-mssql-secret --from-literal=SA_PASSWORD="pa55word!"
kubectl describe secret platforms-mssql-secret
kubectl get secret platforms-mssql-secret -o jsonpath='{.data.*}' | base64 -d

kubectl create secret generic commands-mssql-secret --from-literal=SA_PASSWORD="pa55word!"
kubectl describe secret commands-mssql-secret
kubectl get secret commands-mssql-secret -o jsonpath='{.data.*}' | base64 -d

kubectl apply -f platforms-local-pvc.yaml
kubectl apply -f platforms-mssql-depl.yaml

kubectl apply -f commands-local-pvc.yaml
kubectl apply -f commands-mssql-depl.yaml

kubectl create secret generic <secret_name> --from-literal=<key>=<value>
kubectl get secrets
kubectl describe secret <secret_name>
kubectl delete secret <secret-name>
kubectl delete secret --all
kubectl get pvc

# Configuring RabbitMQ for Async Data Service 

# Configuring GRPC for Sync Data Service

