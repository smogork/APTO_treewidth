#! /usr/bin/python

import os
import sys

if __name__ == "__main__":
    v = 2000
    b = 1000
    tws = range(2,10)

    if len(sys.argv) < 2:
        exit(1)

    output_folder = sys.argv[1]

    for tw in tws:
        print(tw)
        
        args = f'{v} {b} -t {tw}'
        cmd = f'./Randomizer/bin/Debug/net5.0/Randomizer {args} > {output_folder}/g_{v}_{b}_{tw}.gr'

        os.system(cmd)
