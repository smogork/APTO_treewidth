#! /usr/bin/python

import sys
import os

if __name__ == '__main__':

    if len(sys.argv) < 3:
        exit(1)

    graphs_folder = sys.argv[1]
    decomposition_folder = sys.argv[2]

    graph_files = onlyfiles = [f for f in os.listdir(graphs_folder) if os.path.isfile(os.path.join(graphs_folder, f))]
    for file in graph_files:
        print(file)

        without_extension = os.path.splitext(file)[0]

        
        output_file = f'{decomposition_folder}/{without_extension}.td'
        cmd = ['./flow-cutter-pace17/flow_cutter_pace17',  f'{os.path.join(graphs_folder, file)}', '>', output_file]
        os.system(f'./htd/build/bin/htd_main < {os.path.join(graphs_folder, file)} > {decomposition_folder}/{without_extension}.td')