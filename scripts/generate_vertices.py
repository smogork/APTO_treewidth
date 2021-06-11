#! /usr/bin/python

import os
import sys

if __name__ == "__main__":
    vs = range(20, 301, 20)
    tw = 5
    
    if len(sys.argv) < 2:
        exit(1)

    output_folder = sys.argv[1]

    for v in vs:
        print(v)
        
        args = f'{v} {int(v/5)} -t {tw}'
        cmd = f'dotnet run --no-build -p /home/oskar/RiderProjects/APTO_treewidth/Randomizer -- {args} > {output_folder}/g_{v}_{int(v/5)}_{tw}.gr'

        os.system(cmd)
