import illustris_python as il
from matplotlib import cm

def make_xyzrgb(snapnum, parttype, field, cmap, roomsize=50., basePath = './LowResData'):
	particles = il.snapshot.loadSubset(basePath,135,parttype,fields=['Coordinates',field])
	data = np.ndarray([len(particles),6])

	if 'Photometrics' in field:
		# bands = ['u','b','v','k','g','r','i','z']
		fielddata = 10**(-0.4*particles[field][2])
	else:
		fielddata = particles[field]
	norm = cm.LogNorm(max(fielddata.min(),1), fielddata.max()) #to make sure all vals positive
	m = cm.ScalarMappable(norm = norm, cmap = cmap)

	data[:,0:3] = particles['Coordinates']
	data[:,3:] = m.to_rgba(fielddata)[:,0:3]
	data[:,0] -= data[:,0].mean()
	data[:,1] -= data[:,1].mean()
	data[:,2] -= data[:,2].mean()
	dx = data[:,0].max() - data[:,0].min()
	dy = data[:,1].max() - data[:,1].min()
	dz = data[:,2].max() - data[:,2].min()
	data[:,0]*=roomsize/dx
	data[:,1]*=roomsize/dy
	data[:,2]*=roomsize/dz
	np.savetxt(basePath+'/parttype_%s_field_%s_snapnum_%d.txt' % (parttype, field, snapnum), data, delimiter=' ')


def lowres():
	for snapnum in range(1, 135, 9):
		make_xyzrgb(snapnum, 'stars', 'GFM_StellarPhotometrics', cm.RdBu)
		make_xyzrgb(snapnum, 'gas', 'Density', cm.magma)
		make_xyzrgb(snapnum, 'dm', 'Potential', cm.viridis)