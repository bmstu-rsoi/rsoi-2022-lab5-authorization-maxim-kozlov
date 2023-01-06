#!/usr/bin/env bash

IFS="," read -ra HOSTS <<<"$WAIT_HOSTS"
path=$(dirname "$0")


PIDs=()
for host in "${HOSTS[@]}"; do
  echo "$host/manage/health"
  "$path"/wait-for.sh -t 120 "$host/manage/health" -- echo "Host $host is active" &
  PIDs+=($!)
done

for pid in "${PIDs[@]}"; do
  if ! wait "${pid}"; then
    exit 1
  fi
done
