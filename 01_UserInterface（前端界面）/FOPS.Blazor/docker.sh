ver='1.0.0-beta.1'
dotnet publish -c release
cd bin/release/net5.0
docker build -t farseernet/fops:${ver} --network=host .
docker push farseernet/fops:${ver}

docker tag farseernet/fops:${ver} farseernet/fops:latest
docker push farseernet/fops:latest