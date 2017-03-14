FROM golang:1.7.4

RUN mkdir -p /go/src/github.com/branch-app/service-xboxlive
WORKDIR /go/src/github.com/branch-app/service-xboxlive
COPY . /go/src/github.com/branch-app/service-xboxlive

RUN go get github.com/tools/godep

RUN godep restore

RUN go install github.com/branch-app/service-xboxlive
EXPOSE 3000
CMD ['/go/bin/service-xboxlive']
