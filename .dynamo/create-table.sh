aws dynamodb create-table \
  --table-name ChameleonData \
  --attribute-definitions \
      AttributeName=RoomCode,AttributeType=S \
  --key-schema AttributeName=RoomCode,KeyType=HASH \
  --provisioned-throughput ReadCapacityUnits=1,WriteCapacityUnits=1 \
  --endpoint-url http://localhost:8000
