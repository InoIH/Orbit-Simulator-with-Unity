import numpy as np
import matplotlib.pyplot as plt
from matplotlib.ticker import FuncFormatter

# Constants
G = 6.67430e-11  
planet_mass = 5.972e13  
mu = G * planet_mass  
dt = 0.1  

# Initial conditions
position = np.array([50.0, 0.0, 0.0])  # meters
velocity = np.array([0.0, 0.0, 10.0])  # m/s
satellite_mass = 1.0  # kg (set to 1 for specific quantities)

# Storage for results
time_points = []
energies = []
linear_momenta = []
angular_momenta_mag = []
positions_history = []

# Helper function for Euler integration (matching your Unity code)
def euler_step(pos, vel):
    r_vec = -pos  # direction from satellite to planet (planet at origin)
    r = np.linalg.norm(r_vec)
    acceleration_mag = mu / (r * r)
    acceleration_vec = acceleration_mag * (r_vec / r)  # direction.normalized
    
    # Euler integration
    new_vel = vel + acceleration_vec * dt
    new_pos = pos + new_vel * dt
    
    return new_pos, new_vel

# Function to compute orbital quantities
def compute_quantities(pos, vel, m):
    r = np.linalg.norm(pos)
    v = np.linalg.norm(vel)
    
    # Specific mechanical energy (per unit mass)
    specific_energy = 0.5 * v**2 - mu / r
    total_energy = m * specific_energy
    
    # Linear momentum (vector and magnitude)
    linear_momentum_vec = m * vel
    linear_momentum_mag = np.linalg.norm(linear_momentum_vec)
    
    # Angular momentum (vector and magnitude)
    angular_momentum_vec = m * np.cross(pos, vel)
    angular_momentum_mag = np.linalg.norm(angular_momentum_vec)
    
    return total_energy, linear_momentum_mag, angular_momentum_mag

# Run simulation
num_steps = 1000000
initial_radius = np.linalg.norm(position)

# Track for period detection (for x-axis in revolutions)
crossings = []  # When satellite crosses positive x-axis (y=0, x>0)
last_x_sign = 1 if position[0] >= 0 else -1

print("Running simulation")
for i in range(num_steps):
    # Store current state
    time_points.append(i * dt)
    
    # Compute quantities
    energy, lin_mom, ang_mom = compute_quantities(position, velocity, satellite_mass)
    energies.append(energy)
    linear_momenta.append(lin_mom)
    angular_momenta_mag.append(ang_mom)
    positions_history.append(position.copy())
    
    # Detect orbit crossings for period calculation
    current_x_sign = 1 if position[0] >= 0 else -1
    # Crossing from negative to positive x (y ~ 0)
    if last_x_sign < 0 and current_x_sign > 0 and abs(position[1]) < 1.0:
        if len(crossings) == 0 or (i * dt - crossings[-1]) > 5:  # Avoid duplicates
            crossings.append(i * dt)
    last_x_sign = current_x_sign
    
    # Euler integration step
    position, velocity = euler_step(position, velocity)
    
    # Safety check
    if np.linalg.norm(position) < 1.0:
        print(f"Satellite crashed into planet at time {i*dt:.2f}s")
        break

print(f"Simulation completed. Detected {len(crossings)} orbit crossings.")

# Calculate orbital periods
periods = []
if len(crossings) > 1:
    for i in range(1, len(crossings)):
        periods.append(crossings[i] - crossings[i-1])
    avg_period = np.mean(periods)
    print(f"Average orbital period: {avg_period:.2f} seconds")
else:
    avg_period = 50  # Rough estimate if not enough crossings
    print("Not enough orbit crossings detected, using estimated period")

# Convert time to revolutions
if len(crossings) > 1:
    # Use detected crossings for accurate revolution count
    revolutions = []
    current_rev = 0
    crossing_idx = 0
    
    for t in time_points:
        if crossing_idx < len(crossings) and t >= crossings[crossing_idx]:
            current_rev = crossing_idx + 1
            crossing_idx += 1
        revolutions.append(current_rev + (t - (crossings[current_rev-1] if current_rev > 0 else 0)) / avg_period)
else:
    # Fallback: estimate using initial conditions
    # For circular orbit: T = 2π√(r³/μ)
    theoretical_period = 2 * np.pi * np.sqrt(initial_radius**3 / mu)
    revolutions = [t / theoretical_period for t in time_points]

# Create the plot
plt.figure(figsize=(12, 8))

# Plot all three quantities
plt.plot(revolutions, energies, 'b-', label='Total Mechanical Energy (J)', linewidth=1.5, alpha=0.8)
plt.plot(revolutions, linear_momenta, 'r-', label='Linear Momentum Magnitude (kg·m/s)', linewidth=1.5, alpha=0.6)
plt.plot(revolutions, angular_momenta_mag, 'g-', label='Angular Momentum Magnitude (kg·m²/s)', linewidth=1.5, alpha=0.6)

plt.xlabel('Number of Revolutions', fontsize=12)
plt.ylabel('Physical Quantities', fontsize=12)
plt.title('Orbital Quantities vs. Number of Revolutions (Euler Integration)', fontsize=14, fontweight='bold')

# Add grid and legend
plt.grid(True, alpha=0.3)
plt.legend(loc='best', fontsize=10)

# Set x-axis limits to show first few revolutions clearly
max_rev = min(10, max(revolutions))
plt.xlim(0, 1000)

theoretical_angular_momentum = satellite_mass * initial_radius * 10 
theoretical_linear_momentum = satellite_mass * 10
# Add text box with simulation info
info_text = f'Initial conditions:\n'
info_text += f'Position: ({initial_radius:.1f}, 0, 0) m\n'
info_text += f'Velocity: (0, 0, 10) m/s\n'
info_text += f'μ = {mu:.2f} m³/s²\n'
info_text += f'dt = {dt} s\n'
info_text += f'Initial Energy: {energies[0]:.4f} J\n'
info_text += f'Final energy: {energies[-1]:.4f} J\n'
info_text += f'Energy change: {energies[-1] - energies[0]:.4f} J\n'
info_text += f'Theoretical angular momentum: {theoretical_angular_momentum:.2f} kg·m²/s\n'
info_text += f'Final angular momentum: {angular_momenta_mag[-1]:.2f} kg·m²/s\n'
info_text += f'Angular momentum change: {100*(angular_momenta_mag[-1] - theoretical_angular_momentum)/theoretical_angular_momentum:.4f}%\n'
info_text += f'Theoretical linear momentum: {linear_momenta[0]:.2f} kg·m/s\n'
info_text += f'Final linear momentum: {linear_momenta[-1]:.2f} kg·m/s\n'
info_text += f'Linear momentum change: {100*(linear_momenta[-1] - theoretical_linear_momentum)/theoretical_linear_momentum:.4f}%\n'

plt.annotate(info_text, xy=(0.02, 0.98), xycoords='axes fraction',
             verticalalignment='top', fontsize=9,
             bbox=dict(boxstyle='round', facecolor='wheat', alpha=0.5))

# Add a secondary axis showing time in seconds
if max_rev > 0:
    secax = plt.gca().secondary_xaxis('top', functions=(lambda rev: rev * avg_period, 
                                                         lambda t: t / avg_period))
    secax.set_xlabel('Time (seconds)', fontsize=10)

plt.tight_layout()
plt.show()