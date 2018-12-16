#!/bin/sh
final=$(echo $1 | sed s/^service-//)

cd ./$final
npm start
