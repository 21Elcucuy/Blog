#!/bin/bash
tail -f Logs/log-*.txt | grep "ERROR"
