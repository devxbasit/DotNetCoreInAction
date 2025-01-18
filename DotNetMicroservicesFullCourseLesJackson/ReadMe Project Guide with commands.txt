les jackson microserviess

Create Empty sln file

create 2 webapi
1. PlatformService
2. CommandService


---------------------------------------
In platform service

add packages
AutoMapper.Extensions.Microsoft.DependencyInjection
Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore.Design
Microsoft.EntityFrameworkCore.InMemory
Microsoft.EntityFrameworkCore.sqlserver

Create below models
Platforms.cs

add folder data
add class AppDbcontext
add IPlatformRepo interface and its implementation
add PrepDb - for initial seeding data

Create below dtos
add PlatformReadDto
add PlatformCreateDto

Create folder profiles
Add file PlatformProfiles
configure and register automapper



Add below controllers : 1:42
PlatformController

------------------------------- 2:16
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

kubectl rollout restart deployment platforms-depl
kubectl rollout restart deployment platformnpservice-srv
kubectl rollout restart deployment commands-depl

kubectl delete deployment platforms-depl
kubectl delete service platformnpservice-srv
kubectl delete deployment commands-depl


kubectl version
kubectl get deployemnts
kubectl get pods
kubectl get services
kubectl apply -f < yaml file name>
kubectl delete deployment <deployment name>
kubectl rollout restart deployment <deployment name>


 ------------------------------------------------------
CommandsService
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.InMemory
dotnet add package Automapper.Extensions.Microsoft.DependencyInjection












































