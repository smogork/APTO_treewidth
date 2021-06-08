#! /usr/bin/python

import networkx

import sys

if len(sys.argv) < 3:
    print("Pass path to .graph6 file and output.")
    exit(1)

collection = networkx.read_graph6(sys.argv[1])
for graph in collection:
    if isinstance(graph, int):
        with open(f'{sys.argv[2]}_{1}.gr', 'w+') as output:
            output.write(f'p tw {len(collection.nodes)} {len(collection.edges)}\n')
            for edge in collection.edges:
                output.write(f'{edge[0] + 1} {edge[1] + 1}\n')
    else:
        i = 1
        for graph in collection:
            with open(f'{sys.argv[2]}_{i}.gr', 'w+') as output:
                output.write(f'p tw {len(graph.nodes)} {len(graph.edges)}\n')
                for edge in graph.edges:
                    output.write(f'{edge[0] + 1} {edge[1] + 1}\n')
            i+=1
    break