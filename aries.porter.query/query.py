# -*- coding=UTF-8 -*-
from dapr.ext.grpc import App, InvokeMethodRequest,InvokeMethodResponse
from pyapollo.apollo_client import ApolloClient
from towhee import pipe, ops
import numpy as np

import config
import proto.aries_pb2 as common_pb
import proto.porter_pb2 as porter_pb
import json
import config as conf
print(">>>queryservice is starting!",flush=True)
apollo_client = ApolloClient(app_id=config.APOLLO_APP_ID,
                      cluster=conf.APOLLO_CLUSTER,
                      config_server_url=config.APOLLO_CONFIG_SERVER_URL,
                      timeout=100)
apollo_client.start()
apollo_conf=apollo_client.get_value("queryservice",namespace=config.APOLLO_NAMESPACES)
app = App()
collection_name="porter_info"
@app.method('Porter$Query$GetSimilarRecommendList')
def get_recommend_list(request: InvokeMethodRequest)-> InvokeMethodResponse:
    req = porter_pb.SearchReq()
    req.ParseFromString(request.data)
    print(f">>>{req.Top}")
    print(f">>>{req.Keyword}")
    db_conf = apollo_client.get_value("milvus", namespace=config.APOLLO_NAMESPACES)
    model_name=apollo_client.get_value("model",namespace=config.APOLLO_NAMESPACES)
    recommend_pipe = (
        pipe.input('title')
        .map('title', 'title_vec', ops.text_embedding.dpr(model_name=model_name))
        .map('title_vec', 'title_vec', lambda x: x / np.linalg.norm(x, axis=0))
        .map('title_vec', "result",
             ops.ann_search.milvus_client(host=db_conf.get("host"), port=str(db_conf.get("port")),
                                          user=db_conf.get("user"),password=db_conf.get("password"),
                                          db_name=db_conf.get("db"), collection_name=collection_name,
                                          anns_field='title_vec',
                                          expr="type=='information'",
                                          output_fields=["code", "title"],
                                          limit=req.Top))
        .map("result", "resp", lambda x: [(item[2], item[3], item[1]) for item in x])
        .output("resp")
    )
    try:
        resp = recommend_pipe(req.Keyword).get()[0]
    except Exception as ex:
        print(f">>>{ex}")
    return InvokeMethodResponse(data=common_pb.AriesJsonListResp(
        JsonList=json.dumps(resp)
    ))
@app.method(name="my-method")
def mymethod(request: InvokeMethodRequest) -> InvokeMethodResponse:
    print(request.metadata, flush=True)
    print(request,flush=True)
    req=porter_pb.SearchReq()
    req.ParseFromString(request.data)
    #request.unpack(req)
    print(f"a>>>>Keyword:{req.Keyword}",flush=True)
    print(f"a>>>>Top:{req.Top}", flush=True)
    resp={
        "a":"testXXX"
    }
    return InvokeMethodResponse(data=common_pb.AriesJsonListResp(
        JsonList=json.dumps(resp)
    ))
port=apollo_conf.get("port")
print(f">>>>app:{port}is running",flush=True)
app.run(port)