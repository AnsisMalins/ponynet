from cv import *
from __builtin__ import min,max,sum
import numpy as np
from pickle import load,dump
import gzip
import sys
import os
from matplotlib.pyplot import *

loadgraph = True
loadregions = True

inputfile = "introsmall.png"

if len(sys.argv)>1:
    inputfile = sys.argv[1]

print "Input:"+inputfile
image = imread(inputfile)

newcol = CV_RGB(127,127,127)

lo = 20
up = 20
connectivity = 4
flags = connectivity + (255 << 8) + CV_FLOODFILL_FIXED_RANGE

def fullff(image):
    mask = np.zeros((image.shape[0]+2,image.shape[1]+2),dtype='uint8')

    cvRegion = np.zeros((image.shape[0]+2,image.shape[1]+2),dtype=int)
    filled = image.copy()

    regioninfo=[]
    regionnum=1
    linenum=0
    start = (0,0)

    while 1:
        fillres = FloodFill(filled,start,newcol,CV_RGB( lo, lo, lo ), CV_RGB( up, up, up ), flags, mask)
        regioninfo.append(fillres)
        AddS(cvRegion,1,cvRegion,mask)
        regionnum+=1
        while 1:
            if np.all(mask[linenum]): linenum+=1
            else: break
            if linenum==len(mask): break
        if linenum==len(mask): break
        start = ((mask[linenum]==0).nonzero()[0][0]-1,linenum-1)

    regioninfo.append((0,(0,0,0),(0,0,0,0)))
    regioninfo.reverse()
    return regioninfo,cvRegion

prefix = inputfile.split(".")[0]

if not os.access(prefix+"-regioninfo.pkl.gz",os.R_OK): loadregions = False
if loadregions:
    print "Loading"
    regioninfo = load(gzip.open(prefix+"-regioninfo.pkl.gz","rb"))
    cvRegion = load(gzip.open(prefix+"-cvRegion.pkl.gz","rb"))
    print "Loaded"
else:
    regioninfo,cvRegion = fullff(image)
    print "Saving"
    dump(regioninfo,gzip.open(prefix+"-regioninfo.pkl.gz","wb"))
    dump(cvRegion,gzip.open(prefix+"-cvRegion.pkl.gz","wb"))
    print "Saved"

cvr = cvRegion[1:-1,1:-1]

def mcount(matrix,flatten=True):
    count={}
    if flatten: flat=matrix.flat
    else: flat=matrix
    for x in flat:
        count[x] = count.get(x,0) + 1
    return count

totalsize = image.size/3
bigrcutoff = int(0.0006*totalsize)
medrcutoff = int(0.0001*totalsize)

bigregions = [(i,inf[0]) for i,inf in enumerate(regioninfo) if inf[0]>bigrcutoff]
medregions = [(i,inf[0]) for i,inf in enumerate(regioninfo) if medrcutoff<inf[0]<=bigrcutoff]

def getcol(cvr,rnum):
    collist=map(tuple,image[cvr==rnum])
    colcount=mcount(collist,False)
    colsort=sorted(colcount.items(),key=lambda x:x[1])
    return colsort

def bigregcol(cvr,bigreg):
    brc=[]
    brcd={}
    for rnum,rsize in bigreg:
        cols = getcol(cvr,rnum)
        col,numpix=cols[-1]
        #dangerous for gradients
        totalpix = 0
        for x in reversed(cols):
            if cdist(x[0],col)<200: totalpix += x[1]
            else: break
        brc.append((rnum,col,totalpix/rsize))
        brcd[rnum]=(col,totalpix/rsize)
    return brc,brcd

def cdist(c1,c2):
    return sum((max(c1[i],c2[i])-min(c1[i],c2[i]))**2 for i in xrange(len(c1)))

def colconv(col):
    return (int(col[5:7],16),int(col[3:5],16),int(col[1:3],16))

brc,brcd=bigregcol(cvr,bigregions+medregions)

matchdata = ({'SecondaryColors': ('#33346C', '#6D288C', '#EE4F90'), 'Outlines': ('#AD75C1', '#141F4D', '#010401'), 'Name': 'Twilight', 'PrimaryColors': '#C8A9CF'},
{'SecondaryColors': ('#DC1E38', '#E35B29', '#FFFFC1', '#65CF4B', '#2D94C1', '#57087B'), 'Outlines': ('#6EAAD7', '#1F97CE', '#01030A'), 'Name': 'Rainbow Dash', 'PrimaryColors': '#ADEFFF'},
{'SecondaryColors': ('#3E2F7F',), 'Outlines': ('#CCD0D8', '#603778', '#83D2EF'), 'Name': 'Rarity', 'PrimaryColors': '#F7FDFF'},
{'SecondaryColors': ('#F867AA',), 'Outlines': ('#F69CC8', '#DA4396'), 'Name': 'Pinkie Pie', 'PrimaryColors': '#FBC5DF'},
{'SecondaryColors': ('#FAF3AD', '#CA9756'), 'Outlines': ('#E46E2B', '#ECD263', '#B2834D'), 'Name': 'Applejack', 'PrimaryColors': '#F9B763'},
{'SecondaryColors': ('#FFBADA',), 'Outlines': ('#EDDB6B', '#F592C5'), 'Name': 'Fluttershy', 'PrimaryColors': '#FDF7A5'})
#, '#010305' (Rarity outline removed)
#, '#030504' (Fluttershy outline removed)

body = {'Fluttershy':(250,250,175)}

manes = {'Rarity':[(82,60,145),(84,60,147),(82,61,148),(85,60,125),(108,50,134)],
         'Fluttershy':[(246,182,208),(233,166,202),(230,165,201)],
         'Pinkie Pie':[(246,66,140),(235,92,155)],
         'Applejack':[(253,246,176),(249,247,175)],
         'Twilight':[(230,70,140),(40,55,115),(100,45,137),(59,75,135)],
         'Rainbow Dash':[(254,247,170),(251,248,174),(242,112,50),(217,89,41),(234,66,65),(98,187,81),(29,152,214),(103,48,137)]}
manes['Rarity'].extend([(104,50,130),(93,30,120)])

manesoutline = {'Rarity':['#603778'],
                'Fluttershy':['#F592C5'],
                'Twilight':['#141F4D'],
                'Applejack':['#ECD263'],
                'Pinkie Pie':['#DA4396'],
                'Rainbow Dash':['#1F97CE']}

matchd = dict((data["Name"],data) for data in matchdata)

def saver(rnum):
    SaveImage("bestreg.png",(cvr==rnum)*255)
    neigh=sum(cvr==x for x in g[rnum])*255
    SaveImage("neigh.png", neigh)

bigrnum=[x[0] for x in bigregions]
bigrnum+=[x[0] for x in medregions]

def addedge(g,v1,v2):
    if v1 not in bigrnum or v2 not in bigrnum:
        return
    g[v1][v2]=g[v1].get(v2,0)+1
    g[v2][v1]=g[v2].get(v1,0)+1

def distadd(g,cvr,xdist=1,ydist=0):
    maxx,maxy=cvr.shape
    consecdiff = (cvr[:maxx-xdist,:maxy-ydist]!=cvr[xdist:,ydist:]).nonzero()
    for i in xrange(len(consecdiff[0])):
        x,y=consecdiff[0][i],consecdiff[1][i]
        addedge(g,cvr[x][y],cvr[x+xdist][y+ydist])

def distadd(g,cvr,xdist=1,ydist=0):
    maxx,maxy=cvr.shape
    consecdiff = (cvr[:maxx-xdist,:maxy-ydist]!=cvr[xdist:,ydist:])
    pairs = np.array((cvr[consecdiff],cvr[xdist:,ydist:][consecdiff])).transpose()
    for a,b in pairs:
        addedge(g,a,b)

def distaddall(g,cvr):
    distadd(g,cvr)
    distadd(g,cvr,0,1)

    distadd(g,cvr,0,2)
    distadd(g,cvr,2,0)
    distadd(g,cvr,1,1)

print "Building graph"

if not os.access(prefix+"-g.pkl.gz",os.R_OK): loadgraph = False

if loadgraph:
    g=load(gzip.open(prefix+"-g.pkl.gz","rb"))
else:
    g={}
    for rnum in bigrnum:
        g[rnum]={}
    distaddall(g,cvr)
    dump(g,gzip.open(prefix+"-g.pkl.gz","wb"))

print "Finished building graph"

#lower is better
def valuefunction(primdist,nhdist):
    big = 10**6
    small = 500
    if len(nhdist)>1: return primdist+nhdist[0]+nhdist[1]
    elif len(nhdist)>0: return primdist+nhdist[0]*2+small
    else: return primdist+big*2

def secvalue(nhd,nh2dist):
    big = 5000
    small = 500
    if len(nh2dist)>1: return nh2dist[0]+nh2dist[1]
    elif len(nh2dist)>0: return nh2dist[0]*2+small
    else: return big*2

def nhval(nh,outlines,rsize):
    rcol,rratio = brcd[nh]
    nhsize = regioninfo[nh][0]
    penalty = max(1,nhsize/rsize)
    return penalty*min(cdist(rcol,outline) for outline in outlines)

def getvalues(pc,outlines,sc,bigreg=None,primonly=False):
    if bigreg is None: bigreg=bigregions
    values = {}
    for rnum,rsize in bigreg:
        rcol,rratio = brcd[rnum]
        primdist = cdist(rcol,pc)/rratio
        nhdist = []
        for nh in g[rnum]:
            rcol,rratio = brcd[nh]
            nhsize = regioninfo[nh][0]
            penalty = max(1,nhsize/rsize)
            nhval = penalty*min(cdist(rcol,outline) for outline in outlines)
            nhdist.append(nhval)
            #outline intersection is greater than outline size
            if g[rnum][nh]>regioninfo[nh][0]:
                nhdist.append(nhval)
        nhdist.sort()
        values[rnum] = valuefunction(primdist,nhdist)
        if primonly: values[rnum]=primdist
    return values

ponyval = {}
for i,data in enumerate(matchdata):
    pc = colconv(data['PrimaryColors'])
    outlines = map(colconv,data['Outlines'])
    sc = map(colconv,data['SecondaryColors'])
    values = getvalues(pc,outlines,sc)
    bestreg,bestval = min(values.items(),key=lambda x:x[1])
    SaveImage("p"+str(i)+"-"+data['Name']+".png",(cvr==bestreg)*255)
    print data['Name'].ljust(20),bestreg,bestval
    ponyval[data['Name']] = bestval

bestmane = {}
for pname,pmanes in manes.items():
    bestmane[pname] = "" #infinity
    for mane in pmanes:
        pc = tuple(reversed(mane))
        outlines = map(colconv,manesoutline[pname])
        values = getvalues(pc,outlines,sc,bigregions,False)
        bestreg,bestval = min(values.items(),key=lambda x:x[1])
        print (pname+" Mane").ljust(20),bestreg,bestval
        bestmane[pname] = min(bestval,bestmane[pname])

for pname in manes:
    print (pname+" Total").ljust(20),ponyval[pname]+bestmane[pname]

def printhtml(pname,param):
    print "<tr><td>"+pname+"</td><td>"+param+"</td>"
    data = matchd[pname][param]
    if type(data)==tuple:
        for x in data:
            print '<tr><td>'+x+'</td><td bgcolor="'+x+'" width="100px"></td></tr>'
    else:
        print "<tr><td>"+data+'</td><td bgcolor="'+data+'" width="100px"></td></tr>'
