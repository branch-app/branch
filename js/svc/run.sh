#!/bin/sh
final=$(echo $1 | sed s/^service-//)

cd ./svc/$final
npm start
