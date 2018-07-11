#!/bin/bash
#protoc --descriptor_set_out=. *.proto
filelist=`ls ./*.proto`  
for file in $filelist  
do  
echo $file  
protoc --descriptor_set_out=${file%.*}.pb $file
done  