#!/bin/bash
declare -a a=($(docker ps -q))
docker exec ${a[0]} /sovolcoin/src/sovolcoin-cli -regtest getnewaddress $1
