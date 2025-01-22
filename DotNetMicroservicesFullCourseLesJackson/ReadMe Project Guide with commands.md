
******************************
create 2 WebApis
******************************
1. PlatformService
2. CommandService


******************************
In PlatformService WebApi
******************************
dotnet add AutoMapper.Extensions.Microsoft.DependencyInjection
dotnet add Microsoft.EntityFrameworkCore
dotnet add Microsoft.EntityFrameworkCore.Design
dotnet add Microsoft.EntityFrameworkCore.InMemory
dotnet add Microsoft.EntityFrameworkCore.sqlserver

setup PlatformController, models, dtos, automapper profile, AppDbContext, Initial data seeding, IPlatform Repository

******************************
Docker In Action
******************************
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

******************************
Kubernetes In Action
******************************
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
kubectl get services
kubectl apply -f < yaml file name>
kubectl delete deployment <deployment name>
kubectl rollout restart deployment <deployment name>
kubectl delete deployment --all
kubectl delete services --all
kubectl delete deployment --all --namespace=ingress-nginx
kubectl delete services --all --namespace=ingress-nginx

******************************
In CommandsService WebApi
******************************
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.InMemory
dotnet add package Automapper.Extensions.Microsoft.DependencyInjection

******************************
API Gateway ingress nginx
******************************
https://kubernetes.github.io/ingress-nginx/deploy/#docker-desktop

kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.12.0/deploy/static/provider/cloud/deploy.yaml
kubectl get namespace
kubectl get pods --namespace=ingress-nginx
kubectl apply -f ingress-srv.yaml





-----------------------------------------------------
-----------------------------------------------------
-----------------------------------------------------
Resume from here -> Adding an API Gateway -> https://www.youtube.com/watch?v=DgVjEo3OGBI&t=17095s
facing some issue with ingress-nginx (acme.com/api/platform not working, hosts configured)
api gateway not working correctly
-----------------------------------------------------
-----------------------------------------------------
-----------------------------------------------------
