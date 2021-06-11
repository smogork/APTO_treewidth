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
        b = int(v/5)
        
        args = f'{v} {b} -t {tw}'
        cmd = f'./Randomizer/bin/Debug/net5.0/Randomizer {args} > {output_folder}/g_{v}_{b}_{tw}.gr'

        os.system(cmd)
