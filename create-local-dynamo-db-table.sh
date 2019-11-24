# https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Tools.CLI.html#Tools.CLI.UsingWithDDBLocal
# docker run -p 8000:8000 amazon/dynamodb-local

aws dynamodb list-tables \
    --endpoint-url http://localhost:8000

aws dynamodb create-table \
    --table-name ChameleonData \
    --attribute-definitions \
        AttributeName=RoomCode,AttributeType=S \
    --key-schema AttributeName=RoomCode,KeyType=HASH \
    --provisioned-throughput ReadCapacityUnits=1,WriteCapacityUnits=1 \
    --endpoint-url http://localhost:8000

aws dynamodb list-tables \
    --endpoint-url http://localhost:8000
