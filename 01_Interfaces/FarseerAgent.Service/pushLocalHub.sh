docker kill farseer_agent
docker rm farseer_agent
docker rmi farseernet/farseer_agent:latest

# 编译应用
dotnet publish -c release

# 打包
cd bin/release/net6.0/publish
docker build -t farseernet/farseer_agent:latest --network=host .

# 发到内网
docker login dockerhub.abtest.ws/test -u admin -p admin
docker tag farseernet/farseer_agent:latest dockerhub.abtest.ws/test:farseer_agent-dev2
docker push dockerhub.abtest.ws/test:farseer_agent-dev2
docker rmi dockerhub.abtest.ws/test:farseer_agent-dev2

docker run -d --name farseer_agent \
farseernet/farseer_agent:latest \
-v /var/run/docker.sock:/var/run/docker.sock \
--restart=always