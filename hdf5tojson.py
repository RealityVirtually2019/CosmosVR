import illustris_python as il
import pandas

for i in range(1,136): 
	os.mkdir('snapdir_%d' % i) 

basePath = './LowResData'
stars = il.snapshot.loadSubset(basePath,135,'gas',fields=['Coordinates','Masses','GFM_StellarPhotometrics'])
bands = ['u','b','v','k','g','r','i','z']
df = pandas.DataFrame(data=10**(-0.4*stars['GFM_StellarPhotometrics'][2])) #luminosity from V band magnitude
df.insert(0, stars['Masses']*1e10/.7)
df.insert(0, stars['Coordinates']/.7)
df.columns=['X','Y','Z','Mass','VBandLum']
df.to_json(basePath+'/stars_t=13_7Gyr.json', orient='index') #or orient = 'columns'