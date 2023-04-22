#!/usr/bin/env bash

GRPCSERVER=localhost:5041
PROTODIR=../Protos

echo "List users. Empty balances."
grpcurl -import-path $PROTODIR -proto billing.proto -plaintext -d \
'{}' $GRPCSERVER billing.Billing/ListUsers

echo "Emission 10 coins."
grpcurl -import-path $PROTODIR -proto billing.proto -plaintext -d \
'{"amount": "10"}' $GRPCSERVER billing.Billing/CoinsEmission

echo "List users. After the emission."
grpcurl -import-path $PROTODIR -proto billing.proto -plaintext -d \
'{}' $GRPCSERVER billing.Billing/ListUsers

echo "Move 5 coins."
grpcurl -import-path $PROTODIR -proto billing.proto -plaintext -d \
'{"amount":"5","dst_user":"maria","src_user":"boris"}' $GRPCSERVER billing.Billing/MoveCoins

echo "List users. After the coins movement."
grpcurl -import-path $PROTODIR -proto billing.proto -plaintext -d \
'{}' $GRPCSERVER billing.Billing/ListUsers

echo "A coin with the longest history."
grpcurl -import-path $PROTODIR -proto billing.proto -plaintext -d \
'{}' $GRPCSERVER billing.Billing/LongestHistoryCoin