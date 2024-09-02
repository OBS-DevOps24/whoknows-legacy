#!/bin/bash

PYTHON_SCRIPT_PATH="$1"

# TMP="This variable might become useful at some point. Otherwise delete it." 

while true
do
    if ! python "$PYTHON_SCRIPT_PATH"; then
        echo "Script crashed with exit code $?. Restarting..." >&2
        sleep 1
    fi
done