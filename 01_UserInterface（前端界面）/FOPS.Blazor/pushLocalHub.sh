docker kill fops
docker rm fops
docker rmi fops:latest

dotnet publish -c release
cd bin/release/net5.0/publish
docker build -t fops:latest --network=host .

docker run -d --name fops -p 88:88 \
-e Database__Items__0__Server=docker.for.mac.host.internal \
-e FSS__Server__0__Url=http://docker.for.mac.host.internal:888 \
-e ElasticSearch__0__Server=http://docker.for.mac.host.internal:9200 \
--network=net fops:latest \
 --restart=always