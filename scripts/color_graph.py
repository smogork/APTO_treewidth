#! /usr/bin/python

import sys
import os

if __name__ == '__main__':

    if len(sys.argv) < 3:
        print('wrong arguments')
        exit(1)

    graphs_folder = sys.argv[1]
    decomposition_folder = sys.argv[2]

    graph_files = onlyfiles = [f for f in os.listdir(graphs_folder, ) if os.path.isfile(os.path.join(graphs_folder, f))]
    graph_files = sorted(graph_files, key=lambda x: int(x.split('_')[2]))
    print(graph_files)
    for file in graph_files:
        #print(file)

        without_extension = os.path.splitext(file)[0]

        args = f'{os.path.join(decomposition_folder, without_extension+".td")} {os.path.join(graphs_folder, file)} -c 5'
        cmd = f'./Coloring/bin/Debug/net5.0/Coloring {args}'
        wrapper = f'./timer.py "{cmd}"' 
        
        os.system(f'./timer.py \"./Coloring/bin/Debug/net5.0/Coloring {os.path.join(decomposition_folder, without_extension+".td")} {os.path.join(graphs_folder, file)} -c 10\"')