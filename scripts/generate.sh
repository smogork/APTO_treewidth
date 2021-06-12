cd ..
for i in {1..20}
do
    python3 timer.py "dotnet /home/oskar/RiderProjects/APTO_treewidth/Coloring/bin/Debug/net5.0/Coloring.dll /home/oskar/RiderProjects/APTO_treewidth/decompositions/random/flowcutter/g_30_40.td /home/oskar/RiderProjects/APTO_treewidth/graphs/pace/raport/random/g_30_40.gr -c $i"
done