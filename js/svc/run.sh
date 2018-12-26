#!/bin/sh
final=$(echo $1 | sed s/^service-//)

cd ./svcs/$final
npm start
