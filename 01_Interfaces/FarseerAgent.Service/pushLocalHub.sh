docker kill farseer_agent
docker rm farseer_agent
docker rmi farseernet/farseer_agent:latest

dotnet publish -c release
cd bin/release/net6.0/publish
docker build -t farseernet/farseer_agent:latest --network=host .

docker run -d --name farseer_agent \
farseernet/farseer_agent:latest \
-v /var/run/docker.sock:/var/run/docker.sock \
--restart=always