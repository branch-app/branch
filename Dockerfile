FROM golang:1.7.5-wheezy

WORKDIR /go/src/github.com/branch-app/service-xboxlive
Add . /go/src/github.com/branch-app/service-xboxlive/

RUN go get github.com/tools/godep

RUN godep restore

RUN go install github.com/branch-app/service-xboxlive
EXPOSE 3000
ENTRYPOINT ["/go/bin/service-xboxlive"]
