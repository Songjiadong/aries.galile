FROM python:3.11.3 AS base
LABEL authors="justin"
EXPOSE 80
EXPOSE 6666
WORKDIR /app

FROM base AS final
WORKDIR /app
ADD ./requirements.txt .
RUN mkdir "/app/whl"
#COPY ["./whl/","/app/whl"]
COPY ["./models/","/app/models"]
COPY ["./*.py", "/app/"]
COPY ["./proto/", "/app/proto"]
RUN pip install --upgrade pip
RUN pip install -r requirements.txt --no-index --find-links /app/whl
RUN pip install /app/whl/pyapollo-0.0.1.dev1-py3-none-any.whl
RUN rm -f /app/whl

ENTRYPOINT [ "python", "query.py" ]