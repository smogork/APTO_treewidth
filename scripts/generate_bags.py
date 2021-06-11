#! /usr/bin/python

import os
import sys

if __name__ == "__main__":
    v = 100
    bs = range(10, 101, 5)
    tw = 10

    if len(sys.argv) < 2:
        exit(1)

    output_folder = sys.argv[1]

    for b in bs:
        print(b)
        
        args = f'{v} {b} -t {tw}'
        cmd = f'./Randomizer/bin/Debug/net5.0/Randomizer {args} > {output_folder}/g_{v}_{b}_{tw}.gr'

        os.system(cmd)
