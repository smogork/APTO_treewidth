#! /usr/bin/python

import networkx
import networkxgmml

import sys

if len(sys.argv) < 3:
    print("Pass path to .graph6 file and output.")
    exit(1)

g = networkx.read_graph6(sys.argv[1])

with open(sys.argv[2], 'w') as output:
    print >>o, "Hello world!"
    networkxgmml.XGMMLWriter(output, g, "lol", False)
