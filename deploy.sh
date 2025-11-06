#!/bin/bash

RESOURCE_GROUP="qmovechallenge"
VM_NAME="vmchallenge"
LOCATION="eastus"
ADMIN_USERNAME="azureuser"
ADMIN_PASSWORD="Challenge1Sprint!"  
PORT=5082

echo "Criando Resource Group..."
az group create --name $RESOURCE_GROUP --location $LOCATION

echo "Criando VM com senha..."
az vm create \
  --resource-group $RESOURCE_GROUP \
  --name $VM_NAME \
  --image Ubuntu2204 \
  --admin-username $ADMIN_USERNAME \
  --authentication-type password \
  --admin-password $ADMIN_PASSWORD \
  --output json

echo "Abrindo porta $PORT..."
az vm open-port --resource-group $RESOURCE_GROUP --name $VM_NAME --port $PORT

IP=$(az vm show -d -g $RESOURCE_GROUP -n $VM_NAME --query publicIps -o tsv)
echo "IP público da VM: $IP"

echo "Deploy concluído."
echo "Conecte-se via SSH: ssh $ADMIN_USERNAME@$IP"
echo "Use a senha que você definiu no script."

