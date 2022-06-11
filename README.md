## FarseerAgent是什么？

自动采集集群中的容器stdout流日志并写入到ES。

监控服务器的资源情况

在每台服务器（节点）上运行的代理程序。

闲时资源占用：CPU 2 m / 内存 77.4 MiB
## 设计目标

快速搭建：`服务端`运行于docker或k8s下，1分钟即可把代理程序署到您的生产环境中

轻量级：`低内存`（闲时：`76m`）、`低CPU消耗`（闲时：`2m`），依赖少。

无侵入：无需修改现有容器中的应用程序代码，自动感知容器并采集日志到ES

可视化：借用FOPS，可以方便查看日志内容。

_告警：触发设定的条件后，能通知到用户端。（如短信、盯盯、邮件） 未实现_
## 运行环境

    Docker （运行在docker或k8s下）
    Net 6.0 （提供docker.hub镜像服务)
    K8s中以DaemonSet方式运行。

`docker运行脚本`

```
docker run -d --name farseer_agent \
farseernet/farseer_agent:latest \
-v /var/run/docker.sock:/var/run/docker.sock \
-e ElasticSearch__log_es="Server=http://es:9200,Username=es,Password=123456,ReplicasCount=1,ShardsCount=1,RefreshInterval=5,IndexFormat=yyyy_MM" \
--restart=always
```

`环境变量解释`

`ElasticSearch__log_es` es地址，用于写入日志


## 日志查看
FOPS方式：https://github.com/FarseerNet/FOPS

kibana方式：用户自动安装
