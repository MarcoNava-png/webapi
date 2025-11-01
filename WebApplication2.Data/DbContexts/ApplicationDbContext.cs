using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Core.Models;

namespace WebApplication2.Data.DbContexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string, IdentityUserClaim<string>,
        IdentityUserRole<string>, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        private readonly string _currentUserId;

        public ApplicationDbContext(IHttpContextAccessor httpContextAccessor)
        {
            _currentUserId = httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
        }

        public ApplicationDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _currentUserId = httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
        }

        public virtual DbSet<Estado> Estados { get; set; }
        public virtual DbSet<Municipio> Municipios { get; set; }
        public virtual DbSet<CodigoPostal> CodigosPostales { get; set; }
        public virtual DbSet<Aspirante> Aspirante { get; set; }
        public virtual DbSet<AspiranteConvenio> AspiranteConvenio { get; set; }
        public virtual DbSet<AspiranteEstatus> AspiranteEstatus { get; set; }
        public virtual DbSet<Campus> Campus { get; set; }
        public virtual DbSet<Convenio> Convenio { get; set; }
        public virtual DbSet<ConvenioAlcance> ConvenioAlcance { get; set; }
        public virtual DbSet<DiaSemana> DiaSemana { get; set; }
        public virtual DbSet<Direccion> Direccion { get; set; }
        public virtual DbSet<EstadoCivil> EstadoCivil { get; set; }
        public virtual DbSet<Estudiante> Estudiante { get; set; }
        public virtual DbSet<EstudiantePlan> EstudiantePlan { get; set; }
        public virtual DbSet<Genero> Genero { get; set; }
        public virtual DbSet<Grupo> Grupo { get; set; }
        public virtual DbSet<GrupoMateria> GrupoMateria { get; set; }
        public virtual DbSet<Horario> Horario { get; set; }
        public virtual DbSet<Inscripcion> Inscripcion { get; set; }
        public virtual DbSet<Materia> Materia { get; set; }
        public virtual DbSet<MateriaPlan> MateriaPlan { get; set; }
        public virtual DbSet<MedioContacto> MedioContacto { get; set; }
        public virtual DbSet<NivelEducativo> NivelEducativo { get; set; }
        public virtual DbSet<Periodicidad> Periodicidad { get; set; }
        public virtual DbSet<PeriodoAcademico> PeriodoAcademico { get; set; }
        public virtual DbSet<Persona> Persona { get; set; }
        public virtual DbSet<PlanEstudios> PlanEstudios { get; set; }
        public virtual DbSet<Profesor> Profesor { get; set; }
        public virtual DbSet<Turno> Turno { get; set; }
        public virtual DbSet<AspiranteBitacoraSeguimiento> AspiranteBitacoraSeguimiento { get; set; }
        public virtual DbSet<Parciales> Parciales { get; set; }
        public virtual DbSet<CalificacionParcial> CalificacionesParciales { get; set; }
        public virtual DbSet<CalificacionDetalle> CalificacionDetalle { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relación Estado -> Municipio (1:N)
            modelBuilder.Entity<Municipio>()
                .HasOne(m => m.Estado)
                .WithMany(e => e.Municipios)
                .HasForeignKey(m => m.EstadoId)
                .IsRequired();

            // Índice para búsquedas por EstadoId
            modelBuilder.Entity<Municipio>()
                .HasIndex(m => m.EstadoId);

            // Relación Municipio -> CodigoPostal (1:N)
            modelBuilder.Entity<CodigoPostal>()
                .HasOne(cp => cp.Municipio)
                .WithMany(m => m.CodigosPostales)
                .HasForeignKey(cp => cp.MunicipioId)
                .IsRequired();

            // Índice para búsquedas por MunicipioId
            modelBuilder.Entity<CodigoPostal>()
                .HasIndex(cp => cp.MunicipioId);

            modelBuilder.Entity<Aspirante>(entity =>
            {
                entity.HasKey(e => e.IdAspirante).HasName("PK__Aspirant__09EE6349C82C95C4");

                entity.Property(e => e.FechaRegistro).HasDefaultValueSql("(sysutcdatetime())");
                entity.Property(e => e.Observaciones).HasMaxLength(250);

                entity.HasOne(d => d.IdAspiranteEstatusNavigation).WithMany(p => p.Aspirante)
                    .HasForeignKey(d => d.IdAspiranteEstatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Aspirante_Estatus");

                entity.HasOne(d => d.IdMedioContactoNavigation).WithMany(p => p.Aspirante)
                    .HasForeignKey(d => d.IdMedioContacto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Aspirante_Medio");

                entity.HasOne(d => d.IdPersonaNavigation).WithMany(p => p.Aspirante)
                    .HasForeignKey(d => d.IdPersona)
                    .HasConstraintName("FK_Aspirante_Persona");

                entity.HasOne(d => d.IdPlanNavigation).WithMany(p => p.Aspirante)
                    .HasForeignKey(d => d.IdPlan)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Aspirante_Plan");
            });

            modelBuilder.Entity<AspiranteConvenio>(entity =>
            {
                entity.HasKey(e => e.IdAspiranteConvenio).HasName("PK__Aspirant__F372F05FD82F39BA");

                entity.HasIndex(e => new { e.IdAspirante, e.IdConvenio }, "UQ_Aspirante_Convenio").IsUnique();

                entity.Property(e => e.Estatus)
                    .HasMaxLength(20)
                    .HasDefaultValue("Pendiente");
                entity.Property(e => e.Evidencia).HasMaxLength(200);
                entity.Property(e => e.FechaAsignacion).HasDefaultValueSql("(sysutcdatetime())");

                entity.HasOne(d => d.IdAspiranteNavigation).WithMany(p => p.AspiranteConvenio)
                    .HasForeignKey(d => d.IdAspirante)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AspConv_Aspirante");

                entity.HasOne(d => d.IdConvenioNavigation).WithMany(p => p.AspiranteConvenio)
                    .HasForeignKey(d => d.IdConvenio)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AspConv_Convenio");
            });

            modelBuilder.Entity<AspiranteEstatus>(entity =>
            {
                entity.HasKey(e => e.IdAspiranteEstatus).HasName("PK__Aspirant__7B8DBE92EB8DEDF7");

                entity.HasIndex(e => e.DescEstatus, "UQ_AspiranteEstatus").IsUnique();

                entity.Property(e => e.DescEstatus).HasMaxLength(50);
            });

            modelBuilder.Entity<Campus>(entity =>
            {
                entity.HasKey(e => e.IdCampus).HasName("PK__Campus__DA49C2DE1E9DB12C");

                entity.HasIndex(e => e.ClaveCampus, "UQ_Campus_Clave").IsUnique();

                entity.HasIndex(e => e.Nombre, "UQ_Campus_Nombre").IsUnique();

                entity.Property(e => e.Activo).HasDefaultValue(true);
                entity.Property(e => e.ClaveCampus).HasMaxLength(20);
                entity.Property(e => e.Nombre).HasMaxLength(120);

                entity.HasOne(d => d.IdDireccionNavigation).WithMany(p => p.Campus)
                    .HasForeignKey(d => d.IdDireccion)
                    .HasConstraintName("FK_Campus_Direccion");
            });

            modelBuilder.Entity<Convenio>(entity =>
            {
                entity.HasKey(e => e.IdConvenio).HasName("PK__Convenio__51CFFF2B890D4417");

                entity.HasIndex(e => e.ClaveConvenio, "UQ__Convenio__A6197EE9ADAF6A0B").IsUnique();

                entity.Property(e => e.Activo).HasDefaultValue(true);
                entity.Property(e => e.ClaveConvenio).HasMaxLength(30);
                entity.Property(e => e.DescuentoPct).HasColumnType("decimal(5, 2)");
                entity.Property(e => e.Monto).HasColumnType("decimal(12, 2)");
                entity.Property(e => e.Nombre).HasMaxLength(120);
                entity.Property(e => e.TipoBeneficio).HasMaxLength(20);
            });

            modelBuilder.Entity<ConvenioAlcance>(entity =>
            {
                entity.HasKey(e => e.IdConvenioAlcance).HasName("PK__Convenio__2A4E02C0B88720E9");

                entity.HasOne(d => d.IdCampusNavigation).WithMany(p => p.ConvenioAlcance)
                    .HasForeignKey(d => d.IdCampus)
                    .HasConstraintName("FK_ConvAlc_Campus");

                entity.HasOne(d => d.IdConvenioNavigation).WithMany(p => p.ConvenioAlcance)
                    .HasForeignKey(d => d.IdConvenio)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ConvAlc_Convenio");

                entity.HasOne(d => d.IdPlanEstudiosNavigation).WithMany(p => p.ConvenioAlcance)
                    .HasForeignKey(d => d.IdPlanEstudios)
                    .HasConstraintName("FK_ConvAlc_Plan");
            });

            modelBuilder.Entity<DiaSemana>(entity =>
            {
                entity.HasKey(e => e.IdDiaSemana).HasName("PK__DiaSeman__7A209B4EBAF307F1");

                entity.HasIndex(e => e.Nombre, "UQ__DiaSeman__75E3EFCF34622C9D").IsUnique();

                entity.Property(e => e.Nombre).HasMaxLength(20);
            });

            modelBuilder.Entity<Direccion>(entity =>
            {
                entity.HasKey(e => e.IdDireccion).HasName("PK__Direccio__1F8E0C76513A158C");

                entity.Property(e => e.Calle).HasMaxLength(100);
                entity.Property(e => e.NumeroExterior).HasMaxLength(10);
                entity.Property(e => e.NumeroInterior).HasMaxLength(10);
            });

            modelBuilder.Entity<EstadoCivil>(entity =>
            {
                entity.HasKey(e => e.IdEstadoCivil).HasName("PK__EstadoCi__889DE1B24D585C92");

                entity.HasIndex(e => e.DescEstadoCivil, "UQ_EstadoCivil").IsUnique();

                entity.Property(e => e.DescEstadoCivil).HasMaxLength(30);
            });

            modelBuilder.Entity<Estudiante>(entity =>
            {
                entity.HasKey(e => e.IdEstudiante).HasName("PK__Estudian__B5007C24138D11BB");

                entity.HasIndex(e => e.Matricula, "UQ__Estudian__0FB9FB4F890AE66D").IsUnique();

                entity.Property(e => e.Activo).HasDefaultValue(true);
                entity.Property(e => e.Email).HasMaxLength(120);
                entity.Property(e => e.FechaIngreso).HasDefaultValueSql("(CONVERT([date],getdate()))");
                entity.Property(e => e.Matricula).HasMaxLength(30);

                entity.HasOne(d => d.IdPersonaNavigation).WithMany(p => p.Estudiante)
                    .HasForeignKey(d => d.IdPersona)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Estudiante_Persona");

                entity.HasOne(d => d.IdPlanActualNavigation).WithMany(p => p.Estudiante)
                    .HasForeignKey(d => d.IdPlanActual)
                    .HasConstraintName("FK_Estudiante_Plan");
            });

            modelBuilder.Entity<EstudiantePlan>(entity =>
            {
                entity.HasKey(e => e.IdEstudiantePlan).HasName("PK__Estudian__1CF83B276A8D49B7");

                entity.HasIndex(e => new { e.IdEstudiante, e.IdPlanEstudios, e.FechaInicio }, "UQ_EstudiantePlan").IsUnique();

                entity.Property(e => e.FechaInicio).HasDefaultValueSql("(CONVERT([date],getdate()))");

                entity.HasOne(d => d.IdEstudianteNavigation).WithMany(p => p.EstudiantePlan)
                    .HasForeignKey(d => d.IdEstudiante)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EstudiantePlan_Estudiante");

                entity.HasOne(d => d.IdPlanEstudiosNavigation).WithMany(p => p.EstudiantePlan)
                    .HasForeignKey(d => d.IdPlanEstudios)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EstudiantePlan_Plan");
            });

            modelBuilder.Entity<Genero>(entity =>
            {
                entity.HasKey(e => e.IdGenero).HasName("PK__Genero__0F8349880F3BC981");

                entity.HasIndex(e => e.DescGenero, "UQ_Genero").IsUnique();

                entity.Property(e => e.DescGenero).HasMaxLength(30);
            });

            modelBuilder.Entity<Grupo>(entity =>
            {
                entity.HasKey(e => e.IdGrupo).HasName("PK__Grupo__303F6FD92351A792");

                entity.HasIndex(e => new { e.IdPlanEstudios, e.IdPeriodoAcademico, e.NumeroCuatrimestre, e.NumeroGrupo }, "UQ_Grupo_Num").IsUnique();

                entity.Property(e => e.CapacidadMaxima).HasDefaultValue((short)40);

                entity.HasOne(d => d.IdPeriodoAcademicoNavigation).WithMany(p => p.Grupo)
                    .HasForeignKey(d => d.IdPeriodoAcademico)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Grupo_Periodo");

                entity.HasOne(d => d.IdPlanEstudiosNavigation).WithMany(p => p.Grupo)
                    .HasForeignKey(d => d.IdPlanEstudios)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Grupo_Plan");

                entity.HasOne(d => d.IdTurnoNavigation).WithMany(p => p.Grupo)
                    .HasForeignKey(d => d.IdTurno)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Grupo_Turno");
            });

            modelBuilder.Entity<GrupoMateria>(entity =>
            {
                entity.HasKey(e => e.IdGrupoMateria).HasName("PK__GrupoMat__9D026FCD2F0EA6B3");

                entity.HasIndex(e => new { e.IdGrupo, e.IdMateriaPlan }, "UQ_GrupoMateria").IsUnique();

                entity.Property(e => e.Aula).HasMaxLength(50);
                entity.Property(e => e.Cupo).HasDefaultValue((short)40);

                entity.HasOne(d => d.IdGrupoNavigation).WithMany(p => p.GrupoMateria)
                    .HasForeignKey(d => d.IdGrupo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GrupoMateria_Grupo");

                entity.HasOne(d => d.IdMateriaPlanNavigation).WithMany(p => p.GrupoMateria)
                    .HasForeignKey(d => d.IdMateriaPlan)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GrupoMateria_MatPlan");

                entity.HasOne(d => d.IdProfesorNavigation).WithMany(p => p.GrupoMateria)
                    .HasForeignKey(d => d.IdProfesor)
                    .HasConstraintName("FK_GrupoMateria_Profesor");
            });

            modelBuilder.Entity<Horario>(entity =>
            {
                entity.HasKey(e => e.IdHorario).HasName("PK__Horario__1539229BCC12B082");

                entity.HasIndex(e => new { e.IdGrupoMateria, e.IdDiaSemana, e.HoraInicio, e.HoraFin }, "UQ_Horario").IsUnique();

                entity.Property(e => e.Aula).HasMaxLength(50);
                entity.Property(e => e.HoraFin).HasPrecision(0);
                entity.Property(e => e.HoraInicio).HasPrecision(0);

                entity.HasOne(d => d.IdDiaSemanaNavigation).WithMany(p => p.Horario)
                    .HasForeignKey(d => d.IdDiaSemana)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Horario_Dia");

                entity.HasOne(d => d.IdGrupoMateriaNavigation).WithMany(p => p.Horario)
                    .HasForeignKey(d => d.IdGrupoMateria)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Horario_GrupoMat");
            });

            modelBuilder.Entity<Inscripcion>(entity =>
            {
                entity.HasKey(e => e.IdInscripcion).HasName("PK__Inscripc__A122F2BF81A0DA45");

                entity.HasIndex(e => new { e.IdEstudiante, e.IdGrupoMateria }, "UQ_Inscripcion").IsUnique();

                entity.Property(e => e.CalificacionFinal).HasColumnType("decimal(4, 1)");
                entity.Property(e => e.Estado)
                    .HasMaxLength(20)
                    .HasDefaultValue("Inscrito");
                entity.Property(e => e.FechaInscripcion).HasDefaultValueSql("(sysutcdatetime())");

                entity.HasOne(d => d.IdEstudianteNavigation).WithMany(p => p.Inscripcion)
                    .HasForeignKey(d => d.IdEstudiante)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inscripcion_Estudiante");

                entity.HasOne(d => d.IdGrupoMateriaNavigation).WithMany(p => p.Inscripcion)
                    .HasForeignKey(d => d.IdGrupoMateria)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inscripcion_GrupoMateria");
            });

            modelBuilder.Entity<Materia>(entity =>
            {
                entity.HasKey(e => e.IdMateria).HasName("PK__Materia__EC17467041102790");

                entity.HasIndex(e => e.Clave, "UQ__Materia__E8181E1169244C5A").IsUnique();

                entity.Property(e => e.Activa).HasDefaultValue(true);
                entity.Property(e => e.Clave).HasMaxLength(30);
                entity.Property(e => e.Creditos).HasColumnType("decimal(4, 1)");
                entity.Property(e => e.Nombre).HasMaxLength(150);
            });

            modelBuilder.Entity<MateriaPlan>(entity =>
            {
                entity.HasKey(e => e.IdMateriaPlan).HasName("PK__MateriaP__216FB17FE2CA7B4E");

                entity.HasIndex(e => new { e.IdPlanEstudios, e.IdMateria }, "UQ_MateriaPlan").IsUnique();

                entity.HasOne(d => d.IdMateriaNavigation).WithMany(p => p.MateriaPlan)
                    .HasForeignKey(d => d.IdMateria)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MateriaPlan_Materia");

                entity.HasOne(d => d.IdPlanEstudiosNavigation).WithMany(p => p.MateriaPlan)
                    .HasForeignKey(d => d.IdPlanEstudios)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MateriaPlan_Plan");
            });

            modelBuilder.Entity<MedioContacto>(entity =>
            {
                entity.HasKey(e => e.IdMedioContacto).HasName("PK__MedioCon__3E86CE3C31C937DB");

                entity.Property(e => e.DescMedio).HasMaxLength(50);
            });

            modelBuilder.Entity<NivelEducativo>(entity =>
            {
                entity.HasKey(e => e.IdNivelEducativo).HasName("PK__NivelEdu__5035CA164D3A42BB");

                entity.HasIndex(e => e.DescNivelEducativo, "UQ_NivelEducativo").IsUnique();

                entity.Property(e => e.DescNivelEducativo).HasMaxLength(50);
            });

            modelBuilder.Entity<Periodicidad>(entity =>
            {
                entity.HasKey(e => e.IdPeriodicidad).HasName("PK__Periodic__DA476CCD8B84E741");

                entity.HasIndex(e => e.DescPeriodicidad, "UQ_Periodicidad").IsUnique();

                entity.Property(e => e.DescPeriodicidad).HasMaxLength(30);
            });

            modelBuilder.Entity<PeriodoAcademico>(entity =>
            {
                entity.HasKey(e => e.IdPeriodoAcademico).HasName("PK__PeriodoA__E57AB387D551DE0A");

                entity.HasIndex(e => e.Clave, "UQ__PeriodoA__E8181E117466779A").IsUnique();

                entity.Property(e => e.Clave).HasMaxLength(30);
                entity.Property(e => e.Nombre).HasMaxLength(100);

                entity.HasOne(d => d.IdPeriodicidadNavigation).WithMany(p => p.PeriodoAcademico)
                    .HasForeignKey(d => d.IdPeriodicidad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Periodo_Periodicidad");
            });

            modelBuilder.Entity<Persona>(entity =>
            {
                entity.HasKey(e => e.IdPersona).HasName("PK__Persona__2EC8D2AC48F4B00B");

                entity.HasIndex(e => e.Curp, "UQ_Persona_CURP").IsUnique();

                entity.HasIndex(e => e.Correo, "UQ_Persona_Email").IsUnique();

                entity.HasIndex(e => e.Rfc, "UQ_Persona_RFC").IsUnique();

                entity.Property(e => e.ApellidoMaterno).HasMaxLength(50);
                entity.Property(e => e.ApellidoPaterno).HasMaxLength(50);
                entity.Property(e => e.Celular).HasMaxLength(20);
                entity.Property(e => e.Correo).HasMaxLength(100);
                entity.Property(e => e.Curp).HasMaxLength(50);
                entity.Property(e => e.Nombre).HasMaxLength(100);
                entity.Property(e => e.Rfc).HasMaxLength(20);
                entity.Property(e => e.Telefono).HasMaxLength(20);

                entity.HasOne(d => d.IdDireccionNavigation).WithMany(p => p.Persona)
                    .HasForeignKey(d => d.IdDireccion)
                    .HasConstraintName("FK_Persona_Direccion");

                entity.HasOne(d => d.IdEstadoCivilNavigation).WithMany(p => p.Persona)
                    .HasForeignKey(d => d.IdEstadoCivil)
                    .HasConstraintName("FK_Persona_EstadoCivil");

                entity.HasOne(d => d.IdGeneroNavigation).WithMany(p => p.Persona)
                    .HasForeignKey(d => d.IdGenero)
                    .HasConstraintName("FK_Persona_Genero");

                entity.HasIndex(p => p.Nombre);
                entity.HasIndex(p => p.ApellidoPaterno);
                entity.HasIndex(p => p.ApellidoMaterno);
                entity.HasIndex(p => p.Curp);
            });

            modelBuilder.Entity<PlanEstudios>(entity =>
            {
                entity.HasKey(e => e.IdPlanEstudios).HasName("PK__PlanEstu__C60618471021EFD8");

                entity.HasIndex(e => e.ClavePlanEstudios, "UQ_PlanEstudios").IsUnique();

                entity.Property(e => e.ClavePlanEstudios).HasMaxLength(100);
                entity.Property(e => e.DuracionMeses).HasDefaultValue(48);
                entity.Property(e => e.MinimaAprobatoriaFinal).HasDefaultValue(70);
                entity.Property(e => e.MinimaAprobatoriaParcial).HasDefaultValue(60);
                entity.Property(e => e.NombrePlanEstudios).HasMaxLength(100);
                entity.Property(e => e.PermiteAdelantar).HasDefaultValue(false);
                entity.Property(e => e.RVOE).HasMaxLength(50);
                entity.Property(e => e.Version).HasMaxLength(10);

                entity.HasOne(d => d.IdCampusNavigation).WithMany(p => p.PlanEstudios)
                    .HasForeignKey(d => d.IdCampus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Plan_Campus");

                entity.HasOne(d => d.IdNivelEducativoNavigation).WithMany(p => p.PlanEstudios)
                    .HasForeignKey(d => d.IdNivelEducativo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Plan_NivelEducativo");

                entity.HasOne(d => d.IdPeriodicidadNavigation).WithMany(p => p.PlanEstudios)
                    .HasForeignKey(d => d.IdPeriodicidad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Plan_Periodicidad");
            });

            modelBuilder.Entity<Profesor>(entity =>
            {
                entity.HasKey(e => e.IdProfesor).HasName("PK__Profesor__C377C3A119E36880");

                entity.HasIndex(e => e.NoEmpleado, "UQ__Profesor__82F7575B30F93488").IsUnique();

                entity.Property(e => e.Activo).HasDefaultValue(true);
                entity.Property(e => e.EmailInstitucional).HasMaxLength(120);
                entity.Property(e => e.NoEmpleado).HasMaxLength(30);

                entity.HasOne(d => d.IdPersonaNavigation).WithMany(p => p.Profesor)
                    .HasForeignKey(d => d.IdPersona)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Profesor_Persona");
            });

            modelBuilder.Entity<Turno>(entity =>
            {
                entity.HasKey(e => e.IdTurno).HasName("PK__Turno__C1ECF79ACE66190F");

                entity.HasIndex(e => e.Clave, "UQ__Turno__E8181E11A9929118").IsUnique();

                entity.Property(e => e.Clave).HasMaxLength(20);
                entity.Property(e => e.Nombre).HasMaxLength(50);
            });

            modelBuilder.Entity<CalificacionDetalle>(entity =>
            {
                entity.Property(p => p.MaxPuntos).HasPrecision(6, 2);
                entity.Property(p => p.PesoEvaluacion).HasPrecision(6, 2);
                entity.Property(p => p.Puntos).HasPrecision(5, 2);
            });
        }

        public override int SaveChanges()
        {
            AuditEntities();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AuditEntities();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void AuditEntities()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                var now = DateTime.UtcNow;

                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = now;
                    entry.Entity.CreatedBy = _currentUserId;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = now;
                    entry.Entity.UpdatedBy = _currentUserId;
                }
            }
        }
    }
}
