import numpy as np
import matplotlib.pyplot as plt

# Constants
G = 6.67430e-11
planet_mass = 5.972e13
mu = G * planet_mass
dt = 0.01
satellite_mass = 1.0

# Common functions
def compute_energy(pos, vel, m):
    r = np.linalg.norm(pos)
    v = np.linalg.norm(vel)
    return m * (0.5 * v**2 - mu / r)

def euler_step(pos, vel):
    r_vec = -pos
    r = np.linalg.norm(r_vec)
    accel = (mu / (r * r)) * (r_vec / r)
    new_vel = vel + accel * dt
    new_pos = pos + new_vel * dt
    return new_pos, new_vel

def velocity_verlet_step(pos, vel):
    # Step 1: Calculate acceleration at current position
    r_vec = -pos
    r = np.linalg.norm(r_vec)
    accel_t = (mu / (r * r)) * (r_vec / r)
    
    # Step 1.5: v(t + dt/2) = v(t) + a(t) * (dt/2)
    vel_half = vel + accel_t * (dt / 2)
    
    # Step 2: x(t + dt) = x(t) + v(t + dt/2) * dt
    pos_new = pos + vel_half * dt
    
    # Step 3: Calculate acceleration at new position
    r_vec_new = -pos_new
    r_new = np.linalg.norm(r_vec_new)
    accel_t_dt = (mu / (r_new * r_new)) * (r_vec_new / r_new)
    
    # Step 4: v(t + dt) = v(t + dt/2) + a(t + dt) * (dt/2)
    vel_new = vel_half + accel_t_dt * (dt / 2)
    
    return pos_new, vel_new

# Simulation
num_steps = 1000000  # ~100 revolutions
initial_pos = np.array([50.0, 0.0, 0.0])
initial_vel = np.array([0.0, 0.0, 10.0])

# Euler simulation
print("Running Euler simulation...")
pos_e, vel_e = initial_pos.copy(), initial_vel.copy()
energies_e = []
revolutions_e = []

for i in range(num_steps):
    energies_e.append(compute_energy(pos_e, vel_e, satellite_mass))
    revolutions_e.append(i * dt / 31.4)  # Approx period
    pos_e, vel_e = euler_step(pos_e, vel_e)

# Velocity Verlet simulation
print("Running Velocity Verlet simulation...")
pos_v, vel_v = initial_pos.copy(), initial_vel.copy()
energies_v = []

for i in range(num_steps):
    energies_v.append(compute_energy(pos_v, vel_v, satellite_mass))
    pos_v, vel_v = velocity_verlet_step(pos_v, vel_v)

# Create plot
plt.figure(figsize=(14, 8))

# Main plot: Energy vs Revolutions
plt.plot(revolutions_e[:len(energies_e)], energies_e, 'r-', 
         label=f'Euler: ΔE = {energies_e[-1] - energies_e[0]:.4e} J', 
         linewidth=1.5, alpha=0.7)
plt.plot(revolutions_e[:len(energies_v)], energies_v, 'b-', 
         label=f'Velocity Verlet: ΔE = {energies_v[-1] - energies_v[0]:.4e} J', 
         linewidth=1.5, alpha=0.7)

plt.xlabel('Revolutions', fontsize=12)
plt.ylabel('Total Mechanical Energy (J)', fontsize=12)
plt.title('Energy Conservation Comparison: Euler vs Velocity Verlet', fontsize=14, fontweight='bold')
plt.grid(True, alpha=0.3)
plt.legend(loc='best', fontsize=10)

# Add horizontal line at initial energy for reference
plt.axhline(y=energies_e[0], color='k', linestyle='--', alpha=0.5, label='Initial Energy')

# Set x-axis to show up to 100 revolutions
plt.xlim(0, 100)

# Add text box with detailed statistics
energy_stats = f'INITIAL ENERGY (both methods): {energies_e[0]:.6f} J\n\n'
energy_stats += f'EULER METHOD:\n'
energy_stats += f'Final Energy: {energies_e[-1]:.6f} J\n'
energy_stats += f'Change: {energies_e[-1] - energies_e[0]:.4e} J\n'
energy_stats += f'Relative: {100*(energies_e[-1] - energies_e[0])/abs(energies_e[0]):.4f}%\n\n'
energy_stats += f'VELOCITY VERLET:\n'
energy_stats += f'Final Energy: {energies_v[-1]:.6f} J\n'
energy_stats += f'Change: {energies_v[-1] - energies_v[0]:.4e} J\n'
energy_stats += f'Relative: {100*(energies_v[-1] - energies_v[0])/abs(energies_v[0]):.4f}%'

plt.annotate(energy_stats, xy=(0.02, 0.02), xycoords='axes fraction',
             verticalalignment='bottom', fontsize=9,
             bbox=dict(boxstyle='round', facecolor='lightblue', alpha=0.7))

# Inset: Zoom on last 10 revolutions to see detail
ax_inset = plt.axes([0.6, 0.6, 0.3, 0.25])
last_rev = 100
last_steps = int(last_rev * 31.4 / dt)  # Convert revolutions to steps

if len(energies_e) > last_steps and len(energies_v) > last_steps:
    rev_range = revolutions_e[-last_steps:]
    ax_inset.plot(rev_range, energies_e[-last_steps:], 'r-', linewidth=1.5, alpha=0.7)
    ax_inset.plot(rev_range, energies_v[-last_steps:], 'b-', linewidth=1.5, alpha=0.7)
    ax_inset.set_title('Last 10 Revolutions (Zoom)')
    ax_inset.set_xlabel('Revolutions')
    ax_inset.set_ylabel('Energy (J)')
    ax_inset.grid(True, alpha=0.3)
    ax_inset.axhline(y=energies_e[0], color='k', linestyle='--', alpha=0.5)

plt.tight_layout()
plt.show()