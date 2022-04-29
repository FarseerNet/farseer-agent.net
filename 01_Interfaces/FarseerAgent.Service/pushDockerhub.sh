ver='1.0.0'
dotnet publish -c release
cd bin/release/net6.0/publish
docker build -t farseernet/farseer_agent:${ver} --network=host .
docker push farseernet/farseer_agent:${ver}

docker tag farseernet/farseer_agent:${ver} farseernet/farseer_agent:latest
docker push farseernet/farseer_agent:latest