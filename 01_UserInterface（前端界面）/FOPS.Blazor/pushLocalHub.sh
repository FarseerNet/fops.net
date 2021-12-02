docker kill fops
docker rm fops
docker rmi fops:latest

dotnet publish -c release
cd bin/release/net5.0/publish
docker build -t fops:latest --network=host .

docker run -d --name fops -p 88:88 \
--network net --network-alias fops fops:latest \
--restart=always