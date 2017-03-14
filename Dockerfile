FROM golang:1.7.4

RUN mkdir -p /go/src/github.com/branch-app/service-xboxlive
WORKDIR /go/src/github.com/branch-app/service-xboxlive
ADD . /go/src/github.com/branch-app/service-xboxlive

RUN go get github.com/tools/godep
RUN godep restore
RUN go install github.com/branch-app/service-xboxlive

ENTRYPOINT /go/bin/service-xboxlive
EXPOSE 3000
