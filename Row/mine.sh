#!/bin/bash
HOME=/home/sovolcoin
for((i=0;i<$2;i++))
do
	/home/sovolcoin/AltcoinGenerator/sovolcoin/src/sovolcoin-cli generatetoaddress 1 $1
done

