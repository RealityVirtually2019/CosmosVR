import illustris_python as il
import pandas

basePath = './LowResData'
stars = il.snapshot.loadSubset(basePath,135,'stars',fields=['Coordinates','Masses','GFM_StellarPhotometrics'])
bands = ['u','b','v','k','g','r','i','z']
df = pandas.DataFrame(data=10**(-0.4*stars['GFM_StellarPhotometrics'][2])) #luminosity from V band magnitude
df.insert(0, stars['Masses']*1e10/.7)
df.insert(0, stars['Coordinates']/.7)
df.columns=['X','Y','Z','Mass','VBandLum']
df['X'] -= df['X'].mean()
df['Y'] -= df['Y'].mean()
df['Z'] -= df['Z'].mean()
df.to_json(basePath+'/stars_t=13_7Gyr.json', orient='index') #or orient = 'columns'