using BevChain.Core;
using BevChain.Portable;
using BevChain.Portable.Logging;
using BevChain.Email;

using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq.Expressions;
using BevChain.Integration.Portable;
using System.Data.Services.Common;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Text;

namespace BevChain.Integrations
{
    [BeterTable(TrackHistory = true)]
    [Table]
    public class As2Connection : IBase
    {
        private bool _Active = true;
        private EntitySet<As2Pipe> _AS2ConnectionAS2Pipes = new EntitySet<As2Pipe>();
        private int _As2ConnectionId;
        private DateTime _TimeStamp;
        private int _ConnectionId;
        private EntityRef<Connection> _Connection;
        private string _AS2Identifier;
        private string _Url;
        private byte[] _PublicCertificate;
        private byte[] _PrivateKey;
        private string _IpIn;
        private string _IpOut;
        private string _CertificatePassword;
        private byte[] _Pfx;
        private int? _UserId;
        private EntityRef<User> _User;
        private DateTime? _DeActivatedOn;

        [Column(IsDbGenerated = true, IsPrimaryKey = true, Storage = "_As2ConnectionId")]
        public int As2ConnectionId
        {
            get
            {
                return this._As2ConnectionId;
            }
            set
            {
                if (this._As2ConnectionId == value)
                    return;
                this._As2ConnectionId = value;
            }
        }

        [Column(CanBeNull = false, DbType = "datetime", Storage = "_TimeStamp", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Description = "When a record was inserted or last updated", Internal = true, Unique = false)]
        public DateTime TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
            }
        }

        [BeterColumn(Internal = false, Unique = true)]
        [Column(CanBeNull = false, DbType = "int", Storage = "_ConnectionId", UpdateCheck = UpdateCheck.Never)]
        public int ConnectionId
        {
            get
            {
                return this._ConnectionId;
            }
            set
            {
                if (this._ConnectionId == value)
                    return;
                if (this._Connection.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._ConnectionId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "ConnectionId", Storage = "_Connection", ThisKey = "ConnectionId")]
        [BeterColumn(Internal = false, Unique = true)]
        public Connection Connection
        {
            get
            {
                return this._Connection.Entity;
            }
            set
            {
                Connection entity = this._Connection.Entity;
                if (entity == value && this._Connection.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._Connection.Entity = (Connection)null;
                    entity.As2Connections.Remove(this);
                }
                this._Connection.Entity = value;
                if (value == null)
                    throw new Exception("'Connection' is a mandatory field and cannot be set to null.");
                value.As2Connections.Add(this);
                this._ConnectionId = value.ConnectionId;
            }
        }

        [Column(CanBeNull = false, DbType = "varchar(250)", Storage = "_AS2Identifier", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public string AS2Identifier
        {
            get
            {
                return this._AS2Identifier;
            }
            set
            {
                this._AS2Identifier = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "varchar(250)", Storage = "_Url", UpdateCheck = UpdateCheck.Never)]
        public string Url
        {
            get
            {
                return this._Url;
            }
            set
            {
                this._Url = value;
            }
        }

        [Column(CanBeNull = true, DbType = "varbinary(max)", Storage = "_PublicCertificate", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public byte[] PublicCertificate
        {
            get
            {
                return this._PublicCertificate;
            }
            set
            {
                this._PublicCertificate = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "varbinary(max)", Storage = "_PrivateKey", UpdateCheck = UpdateCheck.Never)]
        public byte[] PrivateKey
        {
            get
            {
                return this._PrivateKey;
            }
            set
            {
                this._PrivateKey = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "varchar(50)", Storage = "_IpIn", UpdateCheck = UpdateCheck.Never)]
        public string IpIn
        {
            get
            {
                return this._IpIn;
            }
            set
            {
                this._IpIn = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "varchar(50)", Storage = "_IpOut", UpdateCheck = UpdateCheck.Never)]
        public string IpOut
        {
            get
            {
                return this._IpOut;
            }
            set
            {
                this._IpOut = value;
            }
        }

        [Column(CanBeNull = true, DbType = "varchar(50)", Storage = "_CertificatePassword", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public string CertificatePassword
        {
            get
            {
                return this._CertificatePassword;
            }
            set
            {
                this._CertificatePassword = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "varbinary(max)", Storage = "_Pfx", UpdateCheck = UpdateCheck.Never)]
        public byte[] Pfx
        {
            get
            {
                return this._Pfx;
            }
            set
            {
                this._Pfx = value;
            }
        }

        [Column(CanBeNull = true, DbType = "int", Storage = "_UserId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = false)]
        public int? UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                int? nullable1 = this._UserId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._User.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._UserId = value;
            }
        }

        [BeterColumn(Description = "User who created the entry in the table", Internal = true, Unique = false)]
        [Association(IsForeignKey = true, OtherKey = "UserId", Storage = "_User", ThisKey = "UserId")]
        public User User
        {
            get
            {
                return this._User.Entity;
            }
            set
            {
                User entity = this._User.Entity;
                if (entity == value && this._User.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._User.Entity = (User)null;
                    entity.As2Connections.Remove(this);
                }
                this._User.Entity = value;
                if (value != null)
                {
                    value.As2Connections.Add(this);
                    this._UserId = new int?(value.UserId);
                }
                else
                    this._UserId = new int?();
            }
        }

        [Column(CanBeNull = false, Storage = "_Active", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = true, UpdateUnique = true)]
        public bool Active
        {
            get
            {
                return this._Active;
            }
        }

        [Column(CanBeNull = true, Storage = "_DeActivatedOn", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(IgnoreUniqueUpdateCheck = true, Internal = true, Unique = true, UpdateUnique = true)]
        public DateTime? DeActivatedOn
        {
            get
            {
                return this._DeActivatedOn;
            }
        }

        [Association(OtherKey = "AS2ConnectionId", Storage = "_AS2ConnectionAS2Pipes", ThisKey = "As2ConnectionId")]
        public EntitySet<As2Pipe> AS2ConnectionAS2Pipes
        {
            get
            {
                return this._AS2ConnectionAS2Pipes;
            }
        }

        public As2Connection()
        {
        }

        public As2Connection(DateTime TimeStamp, Connection Connection, string AS2Identifier)
        {
            this.TimeStamp = TimeStamp;
            this.Connection = Connection;
            this.AS2Identifier = AS2Identifier;
        }

        public void DeActivate(DateTime deActivatedOn)
        {
            this._Active = false;
            this._DeActivatedOn = new DateTime?(deActivatedOn);
        }

        public void Activate()
        {
            this._Active = true;
            this._DeActivatedOn = new DateTime?();
        }

        public override string ToString()
        {
            return this.ConnectionId.ToString();
        }
    }

    [BeterTable(TrackHistory = true)]
    [Table]
    public class As2Log : IBase
    {
        private bool _Active = true;
        private int _As2LogId;
        private DateTime _TimeStamp;
        private int _As2PipeId;
        private EntityRef<As2Pipe> _As2Pipe;
        private string _Url;
        private string _Host;
        private int? _FileId;
        private EntityRef<File> _File;
        private bool? _Inbound;
        private string _MessageId;
        private string _MdnMessage;
        private string _MdnContent;
        private string _ErrorMessage;
        private bool? _ErrorFound;
        private int? _LogEvent;
        private int? _UserId;
        private EntityRef<User> _User;
        private DateTime? _DeActivatedOn;

        [Column(IsDbGenerated = true, IsPrimaryKey = true, Storage = "_As2LogId")]
        public int As2LogId
        {
            get
            {
                return this._As2LogId;
            }
            set
            {
                if (this._As2LogId == value)
                    return;
                this._As2LogId = value;
            }
        }

        [BeterColumn(Description = "When a record was inserted or last updated", Internal = true, Unique = false)]
        [Column(CanBeNull = false, DbType = "datetime", Storage = "_TimeStamp", UpdateCheck = UpdateCheck.Never)]
        public DateTime TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = false, DbType = "int", Storage = "_As2PipeId", UpdateCheck = UpdateCheck.Never)]
        public int As2PipeId
        {
            get
            {
                return this._As2PipeId;
            }
            set
            {
                if (this._As2PipeId == value)
                    return;
                if (this._As2Pipe.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._As2PipeId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "As2PipeId", Storage = "_As2Pipe", ThisKey = "As2PipeId")]
        [BeterColumn(Internal = false, Unique = false)]
        public As2Pipe As2Pipe
        {
            get
            {
                return this._As2Pipe.Entity;
            }
            set
            {
                As2Pipe entity = this._As2Pipe.Entity;
                if (entity == value && this._As2Pipe.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._As2Pipe.Entity = (As2Pipe)null;
                    entity.As2Logs.Remove(this);
                }
                this._As2Pipe.Entity = value;
                if (value == null)
                    throw new Exception("'As2 Pipe' is a mandatory field and cannot be set to null.");
                value.As2Logs.Add(this);
                this._As2PipeId = value.As2PipeId;
            }
        }

        [Column(CanBeNull = true, DbType = "varchar(250)", Storage = "_Url", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public string Url
        {
            get
            {
                return this._Url;
            }
            set
            {
                this._Url = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "varchar(250)", Storage = "_Host", UpdateCheck = UpdateCheck.Never)]
        public string Host
        {
            get
            {
                return this._Host;
            }
            set
            {
                this._Host = value;
            }
        }

        [Column(CanBeNull = true, DbType = "int", Storage = "_FileId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public int? FileId
        {
            get
            {
                return this._FileId;
            }
            set
            {
                int? nullable1 = this._FileId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._File.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._FileId = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Association(IsForeignKey = true, OtherKey = "FileId", Storage = "_File", ThisKey = "FileId")]
        public File File
        {
            get
            {
                return this._File.Entity;
            }
            set
            {
                File entity = this._File.Entity;
                if (entity == value && this._File.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._File.Entity = (File)null;
                    entity.As2Logs.Remove(this);
                }
                this._File.Entity = value;
                if (value != null)
                {
                    value.As2Logs.Add(this);
                    this._FileId = new int?(value.FileId);
                }
                else
                    this._FileId = new int?();
            }
        }

        [BeterColumn(Description = "If false then outbound", Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "bit", Storage = "_Inbound", UpdateCheck = UpdateCheck.Never)]
        public bool? Inbound
        {
            get
            {
                return this._Inbound;
            }
            set
            {
                this._Inbound = value;
            }
        }

        [Column(CanBeNull = true, DbType = "varchar(500)", Storage = "_MessageId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public string MessageId
        {
            get
            {
                return this._MessageId;
            }
            set
            {
                this._MessageId = value;
            }
        }

        [Column(CanBeNull = true, DbType = "varchar(8000)", Storage = "_MdnMessage", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public string MdnMessage
        {
            get
            {
                return this._MdnMessage;
            }
            set
            {
                this._MdnMessage = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "varchar(8000)", Storage = "_MdnContent", UpdateCheck = UpdateCheck.Never)]
        public string MdnContent
        {
            get
            {
                return this._MdnContent;
            }
            set
            {
                this._MdnContent = value;
            }
        }

        [Column(CanBeNull = true, DbType = "varchar(500)", Storage = "_ErrorMessage", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Description = "Overhead error description", Internal = false, Unique = false)]
        public string ErrorMessage
        {
            get
            {
                return this._ErrorMessage;
            }
            set
            {
                this._ErrorMessage = value;
            }
        }

        [Column(CanBeNull = true, DbType = "bit", Storage = "_ErrorFound", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Description = "External or Internal Error", Internal = false, Unique = false)]
        public bool? ErrorFound
        {
            get
            {
                return this._ErrorFound;
            }
            set
            {
                this._ErrorFound = value;
            }
        }

        [Column(CanBeNull = true, DbType = "int", Storage = "_LogEvent", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public int? LogEvent
        {
            get
            {
                return this._LogEvent;
            }
            set
            {
                this._LogEvent = value;
            }
        }

        [BeterColumn(Internal = true, Unique = false)]
        [Column(CanBeNull = true, DbType = "int", Storage = "_UserId", UpdateCheck = UpdateCheck.Never)]
        public int? UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                int? nullable1 = this._UserId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._User.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._UserId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "UserId", Storage = "_User", ThisKey = "UserId")]
        [BeterColumn(Description = "User who created the entry in the table", Internal = true, Unique = false)]
        public User User
        {
            get
            {
                return this._User.Entity;
            }
            set
            {
                User entity = this._User.Entity;
                if (entity == value && this._User.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._User.Entity = (User)null;
                    entity.As2Logs.Remove(this);
                }
                this._User.Entity = value;
                if (value != null)
                {
                    value.As2Logs.Add(this);
                    this._UserId = new int?(value.UserId);
                }
                else
                    this._UserId = new int?();
            }
        }

        [Column(CanBeNull = false, Storage = "_Active", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = false)]
        public bool Active
        {
            get
            {
                return this._Active;
            }
        }

        [BeterColumn(Internal = true, Unique = false)]
        [Column(CanBeNull = true, Storage = "_DeActivatedOn", UpdateCheck = UpdateCheck.Never)]
        public DateTime? DeActivatedOn
        {
            get
            {
                return this._DeActivatedOn;
            }
        }

        public As2Log()
        {
        }

        public As2Log(DateTime TimeStamp, As2Pipe As2Pipe)
        {
            this.TimeStamp = TimeStamp;
            this.As2Pipe = As2Pipe;
        }

        public void DeActivate(DateTime deActivatedOn)
        {
            this._Active = false;
            this._DeActivatedOn = new DateTime?(deActivatedOn);
        }

        public void Activate()
        {
            this._Active = true;
            this._DeActivatedOn = new DateTime?();
        }
    }

    [Table]
    [BeterTable(TrackHistory = true)]
    public class As2Pipe : IBase
    {
        private bool _Active = true;
        private EntitySet<As2Log> _As2Logs = new EntitySet<As2Log>();
        private int _As2PipeId;
        private DateTime _TimeStamp;
        private int _PipelineId;
        private EntityRef<Pipeline> _Pipeline;
        private int _AS2ConnectionId;
        private EntityRef<As2Connection> _AS2Connection;
        private string _PartnerIdentifier;
        private string _PartnerName;
        private string _PartnerUrl;
        private byte[] _PartnerPublicCertificate;
        private string _PartnerOutboundIp;
        private string _PartnerInboundIp;
        private byte[] _LocalPublicCertificate;
        private byte[] _LocalKey;
        private string _LocalKeyPassword;
        private byte[] _LocalPfx;
        private string _LocalKeySubject;
        private bool? _OutboundSigned;
        private bool? _OutboundEncrypted;
        private bool? _RequestMdn;
        private int? _UserId;
        private EntityRef<User> _User;
        private DateTime? _DeActivatedOn;

        [Column(IsDbGenerated = true, IsPrimaryKey = true, Storage = "_As2PipeId")]
        public int As2PipeId
        {
            get
            {
                return this._As2PipeId;
            }
            set
            {
                if (this._As2PipeId == value)
                    return;
                this._As2PipeId = value;
            }
        }

        [Column(CanBeNull = false, DbType = "datetime", Storage = "_TimeStamp", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Description = "When a record was inserted or last updated", Internal = true, Unique = false)]
        public DateTime TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
            }
        }

        [Column(CanBeNull = false, DbType = "int", Storage = "_PipelineId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = true)]
        public int PipelineId
        {
            get
            {
                return this._PipelineId;
            }
            set
            {
                if (this._PipelineId == value)
                    return;
                if (this._Pipeline.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._PipelineId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "PipelineId", Storage = "_Pipeline", ThisKey = "PipelineId")]
        [BeterColumn(Internal = false, Unique = true)]
        public Pipeline Pipeline
        {
            get
            {
                return this._Pipeline.Entity;
            }
            set
            {
                Pipeline entity = this._Pipeline.Entity;
                if (entity == value && this._Pipeline.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._Pipeline.Entity = (Pipeline)null;
                    entity.AS2Pipes.Remove(this);
                }
                this._Pipeline.Entity = value;
                if (value == null)
                    throw new Exception("'Pipeline' is a mandatory field and cannot be set to null.");
                value.AS2Pipes.Add(this);
                this._PipelineId = value.PipelineId;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = false, DbType = "int", Storage = "_AS2ConnectionId", UpdateCheck = UpdateCheck.Never)]
        public int AS2ConnectionId
        {
            get
            {
                return this._AS2ConnectionId;
            }
            set
            {
                if (this._AS2ConnectionId == value)
                    return;
                if (this._AS2Connection.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._AS2ConnectionId = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Association(IsForeignKey = true, OtherKey = "As2ConnectionId", Storage = "_AS2Connection", ThisKey = "AS2ConnectionId")]
        public As2Connection AS2Connection
        {
            get
            {
                return this._AS2Connection.Entity;
            }
            set
            {
                As2Connection entity = this._AS2Connection.Entity;
                if (entity == value && this._AS2Connection.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._AS2Connection.Entity = (As2Connection)null;
                    entity.AS2ConnectionAS2Pipes.Remove(this);
                }
                this._AS2Connection.Entity = value;
                if (value == null)
                    throw new Exception("'AS2 Connection' is a mandatory field and cannot be set to null.");
                value.AS2ConnectionAS2Pipes.Add(this);
                this._AS2ConnectionId = value.As2ConnectionId;
            }
        }

        [Column(CanBeNull = false, DbType = "varchar(250)", Storage = "_PartnerIdentifier", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public string PartnerIdentifier
        {
            get
            {
                return this._PartnerIdentifier;
            }
            set
            {
                this._PartnerIdentifier = value;
            }
        }

        [Column(CanBeNull = true, DbType = "varchar(250)", Storage = "_PartnerName", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public string PartnerName
        {
            get
            {
                return this._PartnerName;
            }
            set
            {
                this._PartnerName = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "varchar(500)", Storage = "_PartnerUrl", UpdateCheck = UpdateCheck.Never)]
        public string PartnerUrl
        {
            get
            {
                return this._PartnerUrl;
            }
            set
            {
                this._PartnerUrl = value;
            }
        }

        [Column(CanBeNull = true, DbType = "varbinary(max)", Storage = "_PartnerPublicCertificate", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public byte[] PartnerPublicCertificate
        {
            get
            {
                return this._PartnerPublicCertificate;
            }
            set
            {
                this._PartnerPublicCertificate = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "varchar(50)", Storage = "_PartnerOutboundIp", UpdateCheck = UpdateCheck.Never)]
        public string PartnerOutboundIp
        {
            get
            {
                return this._PartnerOutboundIp;
            }
            set
            {
                this._PartnerOutboundIp = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "varchar(50)", Storage = "_PartnerInboundIp", UpdateCheck = UpdateCheck.Never)]
        public string PartnerInboundIp
        {
            get
            {
                return this._PartnerInboundIp;
            }
            set
            {
                this._PartnerInboundIp = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "varbinary(max)", Storage = "_LocalPublicCertificate", UpdateCheck = UpdateCheck.Never)]
        public byte[] LocalPublicCertificate
        {
            get
            {
                return this._LocalPublicCertificate;
            }
            set
            {
                this._LocalPublicCertificate = value;
            }
        }

        [Column(CanBeNull = true, DbType = "varbinary(max)", Storage = "_LocalKey", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Description = "If overwrite is required from common", Internal = false, Unique = false)]
        public byte[] LocalKey
        {
            get
            {
                return this._LocalKey;
            }
            set
            {
                this._LocalKey = value;
            }
        }

        [Column(CanBeNull = true, DbType = "varchar(50)", Storage = "_LocalKeyPassword", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public string LocalKeyPassword
        {
            get
            {
                return this._LocalKeyPassword;
            }
            set
            {
                this._LocalKeyPassword = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "varbinary(max)", Storage = "_LocalPfx", UpdateCheck = UpdateCheck.Never)]
        public byte[] LocalPfx
        {
            get
            {
                return this._LocalPfx;
            }
            set
            {
                this._LocalPfx = value;
            }
        }

        [Column(CanBeNull = true, DbType = "varchar(50)", Storage = "_LocalKeySubject", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public string LocalKeySubject
        {
            get
            {
                return this._LocalKeySubject;
            }
            set
            {
                this._LocalKeySubject = value;
            }
        }

        [Column(CanBeNull = true, DbType = "bit", Storage = "_OutboundSigned", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public bool? OutboundSigned
        {
            get
            {
                return this._OutboundSigned;
            }
            set
            {
                this._OutboundSigned = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "bit", Storage = "_OutboundEncrypted", UpdateCheck = UpdateCheck.Never)]
        public bool? OutboundEncrypted
        {
            get
            {
                return this._OutboundEncrypted;
            }
            set
            {
                this._OutboundEncrypted = value;
            }
        }

        [Column(CanBeNull = true, DbType = "bit", Storage = "_RequestMdn", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public bool? RequestMdn
        {
            get
            {
                return this._RequestMdn;
            }
            set
            {
                this._RequestMdn = value;
            }
        }

        [BeterColumn(Internal = true, Unique = false)]
        [Column(CanBeNull = true, DbType = "int", Storage = "_UserId", UpdateCheck = UpdateCheck.Never)]
        public int? UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                int? nullable1 = this._UserId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._User.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._UserId = value;
            }
        }

        [BeterColumn(Description = "User who created the entry in the table", Internal = true, Unique = false)]
        [Association(IsForeignKey = true, OtherKey = "UserId", Storage = "_User", ThisKey = "UserId")]
        public User User
        {
            get
            {
                return this._User.Entity;
            }
            set
            {
                User entity = this._User.Entity;
                if (entity == value && this._User.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._User.Entity = (User)null;
                    entity.AS2Pipes.Remove(this);
                }
                this._User.Entity = value;
                if (value != null)
                {
                    value.AS2Pipes.Add(this);
                    this._UserId = new int?(value.UserId);
                }
                else
                    this._UserId = new int?();
            }
        }

        [BeterColumn(Internal = true, Unique = true, UpdateUnique = true)]
        [Column(CanBeNull = false, Storage = "_Active", UpdateCheck = UpdateCheck.Never)]
        public bool Active
        {
            get
            {
                return this._Active;
            }
        }

        [Column(CanBeNull = true, Storage = "_DeActivatedOn", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(IgnoreUniqueUpdateCheck = true, Internal = true, Unique = true, UpdateUnique = true)]
        public DateTime? DeActivatedOn
        {
            get
            {
                return this._DeActivatedOn;
            }
        }

        [Association(OtherKey = "As2PipeId", Storage = "_As2Logs", ThisKey = "As2PipeId")]
        public EntitySet<As2Log> As2Logs
        {
            get
            {
                return this._As2Logs;
            }
        }

        public As2Pipe()
        {
        }

        public As2Pipe(DateTime TimeStamp, Pipeline Pipeline, As2Connection AS2Connection, string PartnerIdentifier)
        {
            this.TimeStamp = TimeStamp;
            this.Pipeline = Pipeline;
            this.AS2Connection = AS2Connection;
            this.PartnerIdentifier = PartnerIdentifier;
        }

        public void DeActivate(DateTime deActivatedOn)
        {
            this._Active = false;
            this._DeActivatedOn = new DateTime?(deActivatedOn);
        }

        public void Activate()
        {
            this._Active = true;
            this._DeActivatedOn = new DateTime?();
        }

        public override string ToString()
        {
            return this.PipelineId.ToString();
        }
    }

    [Table]
    [BeterTable(TrackHistory = true)]
    public class AzureConnection : IBase
    {
        private bool _Active = true;
        private EntitySet<ConnectionPipe> _AzureConnectionAzurePipes = new EntitySet<ConnectionPipe>();
        private int _AzureConnectionId;
        private EntityRef<Connection> _Connection;
        private int _ConnectionId;
        private string _StorageName;
        private string _ConnectionString;
        private DateTime _TimeStamp;
        private DateTime? _DeActivatedOn;

        [Column(IsDbGenerated = true, IsPrimaryKey = true, Storage = "_AzureConnectionId")]
        public int AzureConnectionId
        {
            get
            {
                return this._AzureConnectionId;
            }
            set
            {
                if (this._AzureConnectionId == value)
                    return;
                this._AzureConnectionId = value;
            }
        }

        [Column(CanBeNull = false, DbType = "datetime", Storage = "_TimeStamp", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Description = "When a record was inserted or last updated", Internal = true, Unique = false)]
        public DateTime TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
            }
        }

        [Column(CanBeNull = false, DbType = "int", Storage = "_ConnectionId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = true)]
        public int ConnectionId
        {
            get
            {
                return this._ConnectionId;
            }
            set
            {
                if (this._ConnectionId == value)
                    return;
                if (this._Connection.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._ConnectionId = value;
            }
        }

        [BeterColumn(Internal = false, Unique = true)]
        [Association(IsForeignKey = true, OtherKey = "ConnectionId", Storage = "_Connection", ThisKey = "ConnectionId")]
        public Connection Connection
        {
            get
            {
                return this._Connection.Entity;
            }
            set
            {
                Connection entity = this._Connection.Entity;
                if (entity == value && this._Connection.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._Connection.Entity = (Connection)null;
                    entity.AzureConnections.Remove(this);
                }
                this._Connection.Entity = value;
                if (value == null)
                    throw new Exception("'Connection' is a mandatory field and cannot be set to null.");
                value.AzureConnections.Add(this);
                this._ConnectionId = value.ConnectionId;
            }
        }

        [Column(CanBeNull = false, DbType = "varchar(100)", Storage = "_StorageName", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public string StorageName
        {
            get
            {
                return this._StorageName;
            }
            set
            {
                this._StorageName = value;
            }
        }

        [Column(CanBeNull = false, DbType = "varchar(1000)", Storage = "_ConnectionString", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public string ConnectionString
        {
            get
            {
                return this._ConnectionString;
            }
            set
            {
                this._ConnectionString = value;
            }
        }

        [BeterColumn(Internal = true, Unique = true, UpdateUnique = true)]
        [Column(CanBeNull = false, Storage = "_Active", UpdateCheck = UpdateCheck.Never)]
        public bool Active
        {
            get
            {
                return this._Active;
            }
        }

        [Column(CanBeNull = true, Storage = "_DeActivatedOn", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(IgnoreUniqueUpdateCheck = true, Internal = true, Unique = true, UpdateUnique = true)]
        public DateTime? DeActivatedOn
        {
            get
            {
                return this._DeActivatedOn;
            }
        }

        [Association(OtherKey = "AzureConnectionId", Storage = "_AzureConnectionAzurePipes", ThisKey = "AzureConnectionId")]
        public EntitySet<ConnectionPipe> AzureConnectionAzurePipes
        {
            get
            {
                return this._AzureConnectionAzurePipes;
            }
        }

        public AzureConnection()
        {
        }

        public AzureConnection(DateTime TimeStamp, Connection Connection)
        {
            this.TimeStamp = TimeStamp;
            this.Connection = Connection;
        }

        public void DeActivate(DateTime deActivatedOn)
        {
            this._Active = false;
            this._DeActivatedOn = new DateTime?(deActivatedOn);
        }

        public void Activate()
        {
            this._Active = true;
            this._DeActivatedOn = new DateTime?();
        }

        public override string ToString()
        {
            return this.ConnectionId.ToString();
        }
    }

    public class BaseProcessBlock
    {
        private BETERIntegrationLink _db;
        private int _exportedFiles;
        private int _tagsAdded;
        private Execution _execution;

        public Logger Logger
        {
            get
            {
                return this._db.Logger;
            }
        }

        public int ExportedFiles
        {
            get
            {
                return this._exportedFiles;
            }
        }

        public int TagsAdded
        {
            get
            {
                return this._tagsAdded;
            }
        }

        protected Execution Execution
        {
            get
            {
                return this._execution;
            }
        }

        public File File { get; set; }

        public void InitializeProcessBlock(BETERIntegrationLink db)
        {
            this._db = db;
            this._exportedFiles = 0;
            this._tagsAdded = 0;
        }

        public void InitializeExecution(Execution e)
        {
            this._execution = e;
        }

        public virtual void InitializeProcessing(ExecutionEnvironment e)
        {
        }

        public virtual int ProccessFile(Execution e, object content)
        {
            throw new NotImplementedException();
        }

        public virtual void FinalizeProcessing()
        {
        }

        public virtual int ExecuteProcess(Execution e)
        {
            throw new NotImplementedException();
        }

        public void ExportFile(string filename, object content)
        {
            this.ExportFile(new File(filename, content));
        }

        public void ExportFile(string filename, object content, XTag tag)
        {
            File file = new File(filename, content);
            this.AddTagToRoot(file, tag);
            this.ExportFile(file);
        }

        public void ExportFile(string filename, object content, params XTag[] tags)
        {
            File file = new File(filename, content);
            XTag[] xTagArray = tags;
            for (int i = 0; i < (int)xTagArray.Length; i++)
            {
                this.AddTagToRoot(file, xTagArray[i]);
            }
            this.ExportFile(file);
        }

        public void ExportFile(string filename, object content, List<XTag> tags)
        {
            File file = new File(filename, content);
            foreach (XTag tag in tags)
            {
                this.AddTagToRoot(file, tag);
            }
            this.ExportFile(file);
        }

        private void ExportFile(File f)
        {
            f.Execution = this._execution;
            if (this._execution != null)
            {
                f.SourceFile = this.File;
                foreach (Tag t in Enumerable.Select<OutboundInterfaceTag, Tag>(Enumerable.Where<OutboundInterfaceTag>((IEnumerable<OutboundInterfaceTag>)this._execution.Interface.OutboundInterfaceTags, (Func<OutboundInterfaceTag, bool>)(u => u.Active)), (Func<OutboundInterfaceTag, Tag>)(u => u.Tag)))
                    f.AddTag(t, false, (Pipeline)null, this._execution);
            }
            f.ApplySortRules(true);
            this._db.SubmitChanges();
            ++this._exportedFiles;
        }

        public void AddTag(File f, XTag tag)
        {
            this.AddTagToRoot(f, tag);
            f.ApplySortRules(true);
            this._db.SubmitChanges();
            ++this._tagsAdded;
        }

        public void AddTags(File f, List<XTag> tags)
        {
            foreach (XTag tag in tags)
                this.AddTagToRoot(f, tag);
            f.ApplySortRules(true);
            this._db.SubmitChanges();
            this._tagsAdded += Enumerable.Count<XTag>((IEnumerable<XTag>)tags);
        }

        public void AddTags(File f, params XTag[] tags)
        {
            foreach (XTag tag in tags)
                this.AddTagToRoot(f, tag);
            f.ApplySortRules(true);
            this._db.SubmitChanges();
            this._tagsAdded += Enumerable.Count<XTag>((IEnumerable<XTag>)tags);
        }

        private void AddTagToRoot(File f, XTag tag)
        {
            FileTag ft = f.AddTag(this._db.GetTag(tag.TagName), tag.ShowFile, (Pipeline)null, this._execution);
            foreach (XTag tag1 in tag.Tags)
                this.AddTagToSubTag(ft, tag1);
        }

        private void AddTagToSubTag(FileTag ft, XTag tag)
        {
            FileTag ft1 = ft.AddTag(this._db.GetTag(tag.TagName), tag.ShowFile, (Pipeline)null, this._execution);
            foreach (XTag tag1 in tag.Tags)
                this.AddTagToSubTag(ft1, tag1);
        }
    }

    public class BETERIntegrationLink : DataBaseLink
    {
        private static int _commandTimeout = 300;
        protected User _user;
        public Table<As2Connection> As2Connections;
        public Table<As2Log> As2Logs;
        public Table<As2Pipe> AS2Pipes;
        public Table<BinaryFile> BinaryFiles;
        public Table<Broker> Brokers;
        public Table<BrokerLink> BrokerLinks;
        public Table<BrokerTag> BrokerTags;
        public Table<Connection> Connections;
        public Table<Directory> Directories;
        public Table<EmailConnection> EmailConnections;
        public Table<EmailFile> EmailFiles;
        public Table<EmailPipe> EmailPipes;
        public Table<Execution> Executions;
        public Table<ExecutionLog> ExecutionLogs;
        public Table<File> Files;
        public Table<FileDirectory> FileDirectories;
        public Table<FileExecution> FileExecutions;
        public Table<FileTag> FileTags;
        public Table<FtpConnection> FtpConnections;
        public Table<FtpLog> FtpLogs;
        public Table<FtpPipe> FtpPipes;
        public Table<HttpPipe> HttpPipes;
        public Table<InboundPipeTag> InboundPipeTags;
        public Table<InboundPipeTagSortRule> InboundPipeTagSortRules;
        public Table<Interface> Interfaces;
        public Table<InterfaceSortRule> InterfaceSortRules;
        public Table<InterfaceRequest> InterfaceRequests;
        public Table<Library> Libraries;
        public Table<LibraryVersion> LibraryVersions;
        public Table<Log> Logs;
        public Table<OutboundInterfaceTag> OutboundInterfaceTags;
        public Table<OutboundPipeSortRule> OutboundPipeSortRules;
        public Table<OutboundTask> OutboundTasks;
        public Table<Pipeline> Pipelines;
        public Table<ProcessBlock> ProcessBlocks;
        public Table<ProcessTask> ProcessTasks;
        public Table<SecurityPool> SecurityPools;
        public Table<Tag> Tags;
        public Table<Team> Teams;
        public Table<TeamBroker> TeamBrokers;
        public Table<TeamMember> TeamMembers;
        public Table<TextFile> TextFiles;
        public Table<User> Users;
        public Table<XmlFile> XmlFiles;

        public User User
        {
            get
            {
                return this._user;
            }
        }

        public BETERIntegrationLink(ExecutionEnvironment environment, string userName)
          : base(BETERIntegrationLink.GetConnectionString(environment), BETERIntegrationLink._commandTimeout, TimeZoneInfo.Utc)
        {
            this._user = Queryable.SingleOrDefault<User>((IQueryable<User>)this.Users, (Expression<Func<User, bool>>)(n => n.Active && n.Username == userName));
            if (this._user == null)
                throw new Exception("Username does not exist.");
        }

        public BETERIntegrationLink(string connection, string userName)
          : base(connection, TimeZoneInfo.Utc)
        {
            this._user = Queryable.SingleOrDefault<User>((IQueryable<User>)this.Users, (Expression<Func<User, bool>>)(n => n.Active && n.Username == userName));
            if (this._user == null)
                throw new Exception("Username does not exist.");
        }

        public BETERIntegrationLink(ExecutionEnvironment environment)
          : base(BETERIntegrationLink.GetConnectionString(environment), BETERIntegrationLink._commandTimeout, TimeZoneInfo.Utc)
        {
        }

        public BETERIntegrationLink(string connection)
          : base(connection, TimeZoneInfo.Utc)
        {
        }

        public BETERIntegrationLink(string connection, TimeZoneInfo timeZone)
          : base(connection, timeZone)
        {
        }

        public BETERIntegrationLink(string connection, int timeout, TimeZoneInfo timeZone)
          : base(connection, timeout, timeZone)
        {
        }

        public BETERIntegrationLink(string connection, int timeout, TimeZoneInfo timeZone, ref Logger logger)
          : base(connection, timeout, timeZone, ref logger)
        {
        }

        private static string GetConnectionString(ExecutionEnvironment environment)
        {
            switch (environment)
            {
                case ExecutionEnvironment.Development:
                    return "Server=LAPTOP-KUEGLTAG\\SQLEXPRESS2014;Database=WSIntegration; Trusted_Connection = Yes; ";
                case ExecutionEnvironment.Test:
                    return "Server=LAPTOP-KUEGLTAG\\SQLEXPRESS2014;Database=[WS.Integration];Trusted_Connection = Yes; ";
                case ExecutionEnvironment.Staging:
                    throw new NotImplementedException(environment.ToString());
                case ExecutionEnvironment.Production:
                    return "Server=LAPTOP-KUEGLTAG\\SQLEXPRESS2014;Database=[WS.Integration];Trusted_Connection = Yes; ";
                default:
                    throw new NotImplementedException(environment.ToString());
            }
        }

        public Tag GetTag(string tag)
        {
            Tag entity = Queryable.SingleOrDefault<Tag>((IQueryable<Tag>)this.Tags, (Expression<Func<Tag, bool>>)(n => n.Active && n.TagName.ToLower().Replace(" ", "") == tag.ToLower().Replace(" ", "")));
            if (entity == null)
            {
                entity = new Tag(DateTime.UtcNow, Format.SeperateOnProperCase(tag));
                this.Tags.InsertOnSubmit(entity);
                this.SubmitChanges();
            }
            return entity;
        }
    }

    [BeterTable(Description = "Image, PDF, etc.", TrackHistory = true)]
    [Table]
    public class BinaryFile : IBase
    {
        private bool _Active = true;
        private int _BinaryFileId;
        private DateTime _TimeStamp;
        private int _FileId;
        private EntityRef<File> _File;
        private byte[] _BinaryContent;
        private BinaryFileType? _BinaryFileType;
        private string _AsciiRepresentation;
        private int? _UserId;
        private EntityRef<User> _User;
        private DateTime? _DeActivatedOn;

        [Column(IsDbGenerated = true, IsPrimaryKey = true, Storage = "_BinaryFileId")]
        public int BinaryFileId
        {
            get
            {
                return this._BinaryFileId;
            }
            set
            {
                if (this._BinaryFileId == value)
                    return;
                this._BinaryFileId = value;
            }
        }

        [Column(CanBeNull = false, DbType = "datetime", Storage = "_TimeStamp", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Description = "When a record was inserted or last updated", Internal = true, Unique = false)]
        public DateTime TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
            }
        }

        [Column(CanBeNull = false, DbType = "int", Storage = "_FileId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public int FileId
        {
            get
            {
                return this._FileId;
            }
            set
            {
                if (this._FileId == value)
                    return;
                if (this._File.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._FileId = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Association(IsForeignKey = true, OtherKey = "FileId", Storage = "_File", ThisKey = "FileId")]
        public File File
        {
            get
            {
                return this._File.Entity;
            }
            set
            {
                File entity = this._File.Entity;
                if (entity == value && this._File.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._File.Entity = (File)null;
                    entity.BinaryFiles.Remove(this);
                }
                this._File.Entity = value;
                if (value == null)
                    throw new Exception("'File' is a mandatory field and cannot be set to null.");
                value.BinaryFiles.Add(this);
                this._FileId = value.FileId;
            }
        }

        [Column(CanBeNull = true, DbType = "varbinary(max)", Storage = "_BinaryContent", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public byte[] BinaryContent
        {
            get
            {
                return this._BinaryContent;
            }
            set
            {
                this._BinaryContent = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "int", Storage = "_BinaryFileType", UpdateCheck = UpdateCheck.Never)]
        public BinaryFileType? BinaryFileType
        {
            get
            {
                return this._BinaryFileType;
            }
            set
            {
                this._BinaryFileType = value;
            }
        }

        [BeterColumn(Description = "Temp solution for attachments that contains text", Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "varchar(max)", Storage = "_AsciiRepresentation", UpdateCheck = UpdateCheck.Never)]
        public string AsciiRepresentation
        {
            get
            {
                return this._AsciiRepresentation;
            }
            set
            {
                this._AsciiRepresentation = value;
            }
        }

        [Column(CanBeNull = true, DbType = "int", Storage = "_UserId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = false)]
        public int? UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                int? nullable1 = this._UserId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._User.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._UserId = value;
            }
        }

        [BeterColumn(Description = "User who created the entry in the table", Internal = true, Unique = false)]
        [Association(IsForeignKey = true, OtherKey = "UserId", Storage = "_User", ThisKey = "UserId")]
        public User User
        {
            get
            {
                return this._User.Entity;
            }
            set
            {
                User entity = this._User.Entity;
                if (entity == value && this._User.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._User.Entity = (User)null;
                    entity.BinaryFiles.Remove(this);
                }
                this._User.Entity = value;
                if (value != null)
                {
                    value.BinaryFiles.Add(this);
                    this._UserId = new int?(value.UserId);
                }
                else
                    this._UserId = new int?();
            }
        }

        [Column(CanBeNull = false, Storage = "_Active", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = false)]
        public bool Active
        {
            get
            {
                return this._Active;
            }
        }

        [Column(CanBeNull = true, Storage = "_DeActivatedOn", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = false)]
        public DateTime? DeActivatedOn
        {
            get
            {
                return this._DeActivatedOn;
            }
        }

        public BinaryFile()
        {
        }

        public BinaryFile(DateTime TimeStamp, File File)
        {
            this.TimeStamp = TimeStamp;
            this.File = File;
        }

        public void DeActivate(DateTime deActivatedOn)
        {
            this._Active = false;
            this._DeActivatedOn = new DateTime?(deActivatedOn);
        }

        public void Activate()
        {
            this._Active = true;
            this._DeActivatedOn = new DateTime?();
        }

        public override string ToString()
        {
            return this.FileId.ToString();
        }
    }

    [BeterTable(Description = "An information broker, owning files, pools or processes, sometimes reffered to as a partner in EDI terminology", TrackHistory = true)]
    [Table]
    public class Broker : IBase
    {
        private bool _Active = true;
        private EntitySet<BrokerLink> _BrokerLinks = new EntitySet<BrokerLink>();
        private EntitySet<BrokerLink> _LinkedBrokerLinks = new EntitySet<BrokerLink>();
        private EntitySet<BrokerTag> _BrokerTags = new EntitySet<BrokerTag>();
        private EntitySet<Connection> _Connections = new EntitySet<Connection>();
        private EntitySet<Directory> _Directories = new EntitySet<Directory>();
        private EntitySet<FtpConnection> _FtpConnections = new EntitySet<FtpConnection>();
        private EntitySet<Interface> _Interfaces = new EntitySet<Interface>();
        private EntitySet<ProcessBlock> _ProcessBlocks = new EntitySet<ProcessBlock>();
        private EntitySet<SecurityPool> _SecurityPools = new EntitySet<SecurityPool>();
        private EntitySet<Tag> _Tags = new EntitySet<Tag>();
        private EntitySet<TeamBroker> _TeamBrokers = new EntitySet<TeamBroker>();
        private int _BrokerId;
        private DateTime _TimeStamp;
        private string _BrokerName;
        private string _SupportEmailAddress;
        private int? _SupportTelephoneNumber;
        private int? _SupportMobileNumber;
        private string _ContactEmailAddress;
        private bool? _ReceiveNotificationsOn;
        private int? _UserId;
        private EntityRef<User> _User;
        private DateTime? _DeActivatedOn;

        [Column(IsDbGenerated = true, IsPrimaryKey = true, Storage = "_BrokerId")]
        public int BrokerId
        {
            get
            {
                return this._BrokerId;
            }
            set
            {
                if (this._BrokerId == value)
                    return;
                this._BrokerId = value;
            }
        }

        [Column(CanBeNull = false, DbType = "datetime", Storage = "_TimeStamp", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Description = "When a record was inserted or last updated", Internal = true, Unique = false)]
        public DateTime TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
            }
        }

        [BeterColumn(Internal = false, Unique = true)]
        [Column(CanBeNull = false, DbType = "varchar(50)", Storage = "_BrokerName", UpdateCheck = UpdateCheck.Never)]
        public string BrokerName
        {
            get
            {
                return this._BrokerName;
            }
            set
            {
                this._BrokerName = value;
            }
        }

        [BeterColumn(Description = "Used for notifications", Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "varchar(150)", Storage = "_SupportEmailAddress", UpdateCheck = UpdateCheck.Never)]
        public string SupportEmailAddress
        {
            get
            {
                return this._SupportEmailAddress;
            }
            set
            {
                this._SupportEmailAddress = value;
            }
        }

        [Column(CanBeNull = true, DbType = "int", Storage = "_SupportTelephoneNumber", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Description = "Support contact number", Internal = false, Unique = false)]
        public int? SupportTelephoneNumber
        {
            get
            {
                return this._SupportTelephoneNumber;
            }
            set
            {
                this._SupportTelephoneNumber = value;
            }
        }

        [BeterColumn(Description = "Used for notifications", Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "int", Storage = "_SupportMobileNumber", UpdateCheck = UpdateCheck.Never)]
        public int? SupportMobileNumber
        {
            get
            {
                return this._SupportMobileNumber;
            }
            set
            {
                this._SupportMobileNumber = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "varchar(150)", Storage = "_ContactEmailAddress", UpdateCheck = UpdateCheck.Never)]
        public string ContactEmailAddress
        {
            get
            {
                return this._ContactEmailAddress;
            }
            set
            {
                this._ContactEmailAddress = value;
            }
        }

        [Column(CanBeNull = true, DbType = "bit", Storage = "_ReceiveNotificationsOn", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public bool? ReceiveNotificationsOn
        {
            get
            {
                return this._ReceiveNotificationsOn;
            }
            set
            {
                this._ReceiveNotificationsOn = value;
            }
        }

        [BeterColumn(Internal = true, Unique = false)]
        [Column(CanBeNull = true, DbType = "int", Storage = "_UserId", UpdateCheck = UpdateCheck.Never)]
        public int? UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                int? nullable1 = this._UserId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._User.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._UserId = value;
            }
        }

        [BeterColumn(Description = "User who created the entry in the table", Internal = true, Unique = false)]
        [Association(IsForeignKey = true, OtherKey = "UserId", Storage = "_User", ThisKey = "UserId")]
        public User User
        {
            get
            {
                return this._User.Entity;
            }
            set
            {
                User entity = this._User.Entity;
                if (entity == value && this._User.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._User.Entity = (User)null;
                    entity.Brokers.Remove(this);
                }
                this._User.Entity = value;
                if (value != null)
                {
                    value.Brokers.Add(this);
                    this._UserId = new int?(value.UserId);
                }
                else
                    this._UserId = new int?();
            }
        }

        [BeterColumn(Internal = true, Unique = true, UpdateUnique = true)]
        [Column(CanBeNull = false, Storage = "_Active", UpdateCheck = UpdateCheck.Never)]
        public bool Active
        {
            get
            {
                return this._Active;
            }
        }

        [BeterColumn(IgnoreUniqueUpdateCheck = true, Internal = true, Unique = true, UpdateUnique = true)]
        [Column(CanBeNull = true, Storage = "_DeActivatedOn", UpdateCheck = UpdateCheck.Never)]
        public DateTime? DeActivatedOn
        {
            get
            {
                return this._DeActivatedOn;
            }
        }

        [Association(OtherKey = "BrokerId", Storage = "_BrokerLinks", ThisKey = "BrokerId")]
        public EntitySet<BrokerLink> BrokerLinks
        {
            get
            {
                return this._BrokerLinks;
            }
        }

        [Association(OtherKey = "LinkedBrokerId", Storage = "_LinkedBrokerLinks", ThisKey = "BrokerId")]
        public EntitySet<BrokerLink> LinkedBrokerLinks
        {
            get
            {
                return this._LinkedBrokerLinks;
            }
        }

        [Association(OtherKey = "BrokerId", Storage = "_BrokerTags", ThisKey = "BrokerId")]
        public EntitySet<BrokerTag> BrokerTags
        {
            get
            {
                return this._BrokerTags;
            }
        }

        [Association(OtherKey = "BrokerId", Storage = "_Connections", ThisKey = "BrokerId")]
        public EntitySet<Connection> Connections
        {
            get
            {
                return this._Connections;
            }
        }

        [Association(OtherKey = "BrokerId", Storage = "_Directories", ThisKey = "BrokerId")]
        public EntitySet<Directory> Directories
        {
            get
            {
                return this._Directories;
            }
        }

        [Association(OtherKey = "BrokerId", Storage = "_FtpConnections", ThisKey = "BrokerId")]
        public EntitySet<FtpConnection> FtpConnections
        {
            get
            {
                return this._FtpConnections;
            }
        }

        [Association(OtherKey = "BrokerId", Storage = "_Interfaces", ThisKey = "BrokerId")]
        public EntitySet<Interface> Interfaces
        {
            get
            {
                return this._Interfaces;
            }
        }

        [Association(OtherKey = "BrokerId", Storage = "_ProcessBlocks", ThisKey = "BrokerId")]
        public EntitySet<ProcessBlock> ProcessBlocks
        {
            get
            {
                return this._ProcessBlocks;
            }
        }

        [Association(OtherKey = "BrokerId", Storage = "_SecurityPools", ThisKey = "BrokerId")]
        public EntitySet<SecurityPool> SecurityPools
        {
            get
            {
                return this._SecurityPools;
            }
        }

        [Association(OtherKey = "BrokerId", Storage = "_Tags", ThisKey = "BrokerId")]
        public EntitySet<Tag> Tags
        {
            get
            {
                return this._Tags;
            }
        }

        [Association(OtherKey = "BrokerId", Storage = "_TeamBrokers", ThisKey = "BrokerId")]
        public EntitySet<TeamBroker> TeamBrokers
        {
            get
            {
                return this._TeamBrokers;
            }
        }

        public Broker()
        {
        }

        public Broker(DateTime TimeStamp, string BrokerName)
        {
            this.TimeStamp = TimeStamp;
            this.BrokerName = BrokerName;
        }

        public bool HasPermission(User u)
        {
            return Enumerable.Any<TeamBroker>((IEnumerable<TeamBroker>)this.TeamBrokers, (Func<TeamBroker, bool>)(n =>
            {
                if (n.Active && n.Team.Active)
                    return Enumerable.Any<TeamMember>((IEnumerable<TeamMember>)n.Team.TeamMembers, (Func<TeamMember, bool>)(x =>
                    {
                        if (x.Active)
                            return x.MemberId == u.UserId;
                        return false;
                    }));
                return false;
            }));
        }

        public IOrderedEnumerable<Directory> GetDirectories()
        {
            return Enumerable.OrderBy<Directory, string>(Enumerable.Where<Directory>((IEnumerable<Directory>)this.Directories, (Func<Directory, bool>)(u =>
            {
                if (u.Active && u.Tag.Active)
                    return u.ParentDirectory == null;
                return false;
            })), (Func<Directory, string>)(u => u.Tag.TagName));
        }

        public Directory GetDirectoryOrDefault(string name)
        {
            return Enumerable.SingleOrDefault<Directory>((IEnumerable<Directory>)this.Directories, (Func<Directory, bool>)(n =>
            {
                if (n.Active && n.Tag.Active && n.Tag.TagName.ToLower().Replace(" ", "") == name.ToLower().Replace(" ", ""))
                    return !n.ParentDirectoryId.HasValue;
                return false;
            }));
        }

        public string GetDirectoryName()
        {
            return Format.RemoveSpacesAndSpecialCharacters(this.BrokerName);
        }

        public void DeActivate(DateTime deActivatedOn)
        {
            this._Active = false;
            this._DeActivatedOn = new DateTime?(deActivatedOn);
        }

        public void Activate()
        {
            this._Active = true;
            this._DeActivatedOn = new DateTime?();
        }

        public override string ToString()
        {
            return this.BrokerName.ToString();
        }
    }

    [Table]
    [BeterTable(Description = "Brokers connect to each other - two links are created between brokers, one being the broker and the other the linked broker for each", TrackHistory = true)]
    public class BrokerLink : IBase
    {
        private bool _Active = true;
        private int _BrokerLinkId;
        private DateTime _TimeStamp;
        private int _BrokerId;
        private EntityRef<Broker> _Broker;
        private int _LinkedBrokerId;
        private EntityRef<Broker> _LinkedBroker;
        private int? _UserId;
        private EntityRef<User> _User;
        private DateTime? _DeActivatedOn;

        [Column(IsDbGenerated = true, IsPrimaryKey = true, Storage = "_BrokerLinkId")]
        public int BrokerLinkId
        {
            get
            {
                return this._BrokerLinkId;
            }
            set
            {
                if (this._BrokerLinkId == value)
                    return;
                this._BrokerLinkId = value;
            }
        }

        [Column(CanBeNull = false, DbType = "datetime", Storage = "_TimeStamp", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Description = "When a record was inserted or last updated", Internal = true, Unique = false)]
        public DateTime TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = false, DbType = "int", Storage = "_BrokerId", UpdateCheck = UpdateCheck.Never)]
        public int BrokerId
        {
            get
            {
                return this._BrokerId;
            }
            set
            {
                if (this._BrokerId == value)
                    return;
                if (this._Broker.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._BrokerId = value;
            }
        }

        [BeterColumn(Description = "The parent of the link", Internal = false, Unique = false)]
        [Association(IsForeignKey = true, OtherKey = "BrokerId", Storage = "_Broker", ThisKey = "BrokerId")]
        public Broker Broker
        {
            get
            {
                return this._Broker.Entity;
            }
            set
            {
                Broker entity = this._Broker.Entity;
                if (entity == value && this._Broker.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._Broker.Entity = (Broker)null;
                    entity.BrokerLinks.Remove(this);
                }
                this._Broker.Entity = value;
                if (value == null)
                    throw new Exception("'Broker' is a mandatory field and cannot be set to null.");
                value.BrokerLinks.Add(this);
                this._BrokerId = value.BrokerId;
            }
        }

        [Column(CanBeNull = false, DbType = "int", Storage = "_LinkedBrokerId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public int LinkedBrokerId
        {
            get
            {
                return this._LinkedBrokerId;
            }
            set
            {
                if (this._LinkedBrokerId == value)
                    return;
                if (this._LinkedBroker.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._LinkedBrokerId = value;
            }
        }

        [BeterColumn(Description = "The linked child broker", Internal = false, Unique = false)]
        [Association(IsForeignKey = true, OtherKey = "BrokerId", Storage = "_LinkedBroker", ThisKey = "LinkedBrokerId")]
        public Broker LinkedBroker
        {
            get
            {
                return this._LinkedBroker.Entity;
            }
            set
            {
                Broker entity = this._LinkedBroker.Entity;
                if (entity == value && this._LinkedBroker.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._LinkedBroker.Entity = (Broker)null;
                    entity.LinkedBrokerLinks.Remove(this);
                }
                this._LinkedBroker.Entity = value;
                if (value == null)
                    throw new Exception("'Linked Broker' is a mandatory field and cannot be set to null.");
                value.LinkedBrokerLinks.Add(this);
                this._LinkedBrokerId = value.BrokerId;
            }
        }

        [BeterColumn(Internal = true, Unique = false)]
        [Column(CanBeNull = true, DbType = "int", Storage = "_UserId", UpdateCheck = UpdateCheck.Never)]
        public int? UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                int? nullable1 = this._UserId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._User.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._UserId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "UserId", Storage = "_User", ThisKey = "UserId")]
        [BeterColumn(Description = "User who created the entry in the table", Internal = true, Unique = false)]
        public User User
        {
            get
            {
                return this._User.Entity;
            }
            set
            {
                User entity = this._User.Entity;
                if (entity == value && this._User.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._User.Entity = (User)null;
                    entity.BrokerLinks.Remove(this);
                }
                this._User.Entity = value;
                if (value != null)
                {
                    value.BrokerLinks.Add(this);
                    this._UserId = new int?(value.UserId);
                }
                else
                    this._UserId = new int?();
            }
        }

        [BeterColumn(Internal = true, Unique = false)]
        [Column(CanBeNull = false, Storage = "_Active", UpdateCheck = UpdateCheck.Never)]
        public bool Active
        {
            get
            {
                return this._Active;
            }
        }

        [BeterColumn(Internal = true, Unique = false)]
        [Column(CanBeNull = true, Storage = "_DeActivatedOn", UpdateCheck = UpdateCheck.Never)]
        public DateTime? DeActivatedOn
        {
            get
            {
                return this._DeActivatedOn;
            }
        }

        public BrokerLink()
        {
        }

        public BrokerLink(DateTime TimeStamp, Broker Broker, Broker LinkedBroker)
        {
            this.TimeStamp = TimeStamp;
            this.Broker = Broker;
            this.LinkedBroker = LinkedBroker;
        }

        public void DeActivate(DateTime deActivatedOn)
        {
            this._Active = false;
            this._DeActivatedOn = new DateTime?(deActivatedOn);
        }

        public void Activate()
        {
            this._Active = true;
            this._DeActivatedOn = new DateTime?();
        }
    }

    [Table]
    [BeterTable(TrackHistory = true)]
    public class BrokerTag : IBase
    {
        private bool _Active = true;
        private int _BrokerTagId;
        private DateTime _TimeStamp;
        private int _BrokerId;
        private EntityRef<Broker> _Broker;
        private int _TagId;
        private EntityRef<Tag> _Tag;
        private int? _UserId;
        private EntityRef<User> _User;
        private DateTime? _DeActivatedOn;

        [Column(IsDbGenerated = true, IsPrimaryKey = true, Storage = "_BrokerTagId")]
        public int BrokerTagId
        {
            get
            {
                return this._BrokerTagId;
            }
            set
            {
                if (this._BrokerTagId == value)
                    return;
                this._BrokerTagId = value;
            }
        }

        [Column(CanBeNull = false, DbType = "datetime", Storage = "_TimeStamp", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Description = "When a record was inserted or last updated", Internal = true, Unique = false)]
        public DateTime TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
            }
        }

        [Column(CanBeNull = false, DbType = "int", Storage = "_BrokerId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = true)]
        public int BrokerId
        {
            get
            {
                return this._BrokerId;
            }
            set
            {
                if (this._BrokerId == value)
                    return;
                if (this._Broker.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._BrokerId = value;
            }
        }

        [BeterColumn(Internal = false, Unique = true)]
        [Association(IsForeignKey = true, OtherKey = "BrokerId", Storage = "_Broker", ThisKey = "BrokerId")]
        public Broker Broker
        {
            get
            {
                return this._Broker.Entity;
            }
            set
            {
                Broker entity = this._Broker.Entity;
                if (entity == value && this._Broker.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._Broker.Entity = (Broker)null;
                    entity.BrokerTags.Remove(this);
                }
                this._Broker.Entity = value;
                if (value == null)
                    throw new Exception("'Broker' is a mandatory field and cannot be set to null.");
                value.BrokerTags.Add(this);
                this._BrokerId = value.BrokerId;
            }
        }

        [BeterColumn(Internal = false, Unique = true)]
        [Column(CanBeNull = false, DbType = "int", Storage = "_TagId", UpdateCheck = UpdateCheck.Never)]
        public int TagId
        {
            get
            {
                return this._TagId;
            }
            set
            {
                if (this._TagId == value)
                    return;
                if (this._Tag.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._TagId = value;
            }
        }

        [BeterColumn(Internal = false, Unique = true)]
        [Association(IsForeignKey = true, OtherKey = "TagId", Storage = "_Tag", ThisKey = "TagId")]
        public Tag Tag
        {
            get
            {
                return this._Tag.Entity;
            }
            set
            {
                Tag entity = this._Tag.Entity;
                if (entity == value && this._Tag.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._Tag.Entity = (Tag)null;
                    entity.BrokerTags.Remove(this);
                }
                this._Tag.Entity = value;
                if (value == null)
                    throw new Exception("'Tag' is a mandatory field and cannot be set to null.");
                value.BrokerTags.Add(this);
                this._TagId = value.TagId;
            }
        }

        [BeterColumn(Internal = true, Unique = false)]
        [Column(CanBeNull = true, DbType = "int", Storage = "_UserId", UpdateCheck = UpdateCheck.Never)]
        public int? UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                int? nullable1 = this._UserId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._User.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._UserId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "UserId", Storage = "_User", ThisKey = "UserId")]
        [BeterColumn(Description = "User who created the entry in the table", Internal = true, Unique = false)]
        public User User
        {
            get
            {
                return this._User.Entity;
            }
            set
            {
                User entity = this._User.Entity;
                if (entity == value && this._User.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._User.Entity = (User)null;
                    entity.BrokerTags.Remove(this);
                }
                this._User.Entity = value;
                if (value != null)
                {
                    value.BrokerTags.Add(this);
                    this._UserId = new int?(value.UserId);
                }
                else
                    this._UserId = new int?();
            }
        }

        [Column(CanBeNull = false, Storage = "_Active", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = true, UpdateUnique = true)]
        public bool Active
        {
            get
            {
                return this._Active;
            }
        }

        [BeterColumn(IgnoreUniqueUpdateCheck = true, Internal = true, Unique = true, UpdateUnique = true)]
        [Column(CanBeNull = true, Storage = "_DeActivatedOn", UpdateCheck = UpdateCheck.Never)]
        public DateTime? DeActivatedOn
        {
            get
            {
                return this._DeActivatedOn;
            }
        }

        public BrokerTag()
        {
        }

        public BrokerTag(DateTime TimeStamp, Broker Broker, Tag Tag)
        {
            this.TimeStamp = TimeStamp;
            this.Broker = Broker;
            this.Tag = Tag;
        }

        public void DeActivate(DateTime deActivatedOn)
        {
            this._Active = false;
            this._DeActivatedOn = new DateTime?(deActivatedOn);
        }

        public void Activate()
        {
            this._Active = true;
            this._DeActivatedOn = new DateTime?();
        }
    }

    [Table]
    [BeterTable(TrackHistory = true)]
    public class Connection : IBase
    {
        private bool _Active = true;
        private EntitySet<As2Connection> _As2Connections = new EntitySet<As2Connection>();
        private EntitySet<EmailConnection> _EmailConnections = new EntitySet<EmailConnection>();
        private EntitySet<FtpConnection> _FtpConnections = new EntitySet<FtpConnection>();
        private EntitySet<Pipeline> _Pipelines = new EntitySet<Pipeline>();
        private EntitySet<AzureConnection> _AzureConnections = new EntitySet<AzureConnection>();
        private int _ConnectionId;
        private DateTime _TimeStamp;
        private string _ConnectionName;
        private int _BrokerId;
        private EntityRef<Broker> _Broker;
        private string _ConectionDescription;
        private ConnectionType _ConnectionType;
        private int? _UserId;
        private EntityRef<User> _User;
        private DateTime? _DeActivatedOn;

        [Column(IsDbGenerated = true, IsPrimaryKey = true, Storage = "_ConnectionId")]
        public int ConnectionId
        {
            get
            {
                return this._ConnectionId;
            }
            set
            {
                if (this._ConnectionId == value)
                    return;
                this._ConnectionId = value;
            }
        }

        [Column(CanBeNull = false, DbType = "datetime", Storage = "_TimeStamp", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Description = "When a record was inserted or last updated", Internal = true, Unique = false)]
        public DateTime TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
            }
        }

        [Column(CanBeNull = false, DbType = "varchar(50)", Storage = "_ConnectionName", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = true)]
        public string ConnectionName
        {
            get
            {
                return this._ConnectionName;
            }
            set
            {
                this._ConnectionName = value;
            }
        }

        [Column(CanBeNull = false, DbType = "int", Storage = "_BrokerId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = true)]
        public int BrokerId
        {
            get
            {
                return this._BrokerId;
            }
            set
            {
                if (this._BrokerId == value)
                    return;
                if (this._Broker.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._BrokerId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "BrokerId", Storage = "_Broker", ThisKey = "BrokerId")]
        [BeterColumn(Internal = false, Unique = true)]
        public Broker Broker
        {
            get
            {
                return this._Broker.Entity;
            }
            set
            {
                Broker entity = this._Broker.Entity;
                if (entity == value && this._Broker.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._Broker.Entity = (Broker)null;
                    entity.Connections.Remove(this);
                }
                this._Broker.Entity = value;
                if (value == null)
                    throw new Exception("'Broker' is a mandatory field and cannot be set to null.");
                value.Connections.Add(this);
                this._BrokerId = value.BrokerId;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "varchar(250)", Storage = "_ConectionDescription", UpdateCheck = UpdateCheck.Never)]
        public string ConectionDescription
        {
            get
            {
                return this._ConectionDescription;
            }
            set
            {
                this._ConectionDescription = value;
            }
        }

        [Column(CanBeNull = false, DbType = "int", Storage = "_ConnectionType", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public ConnectionType ConnectionType
        {
            get
            {
                return this._ConnectionType;
            }
            set
            {
                this._ConnectionType = value;
            }
        }

        [BeterColumn(Internal = true, Unique = false)]
        [Column(CanBeNull = true, DbType = "int", Storage = "_UserId", UpdateCheck = UpdateCheck.Never)]
        public int? UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                int? nullable1 = this._UserId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._User.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._UserId = value;
            }
        }

        [BeterColumn(Description = "User who created the entry in the table", Internal = true, Unique = false)]
        [Association(IsForeignKey = true, OtherKey = "UserId", Storage = "_User", ThisKey = "UserId")]
        public User User
        {
            get
            {
                return this._User.Entity;
            }
            set
            {
                User entity = this._User.Entity;
                if (entity == value && this._User.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._User.Entity = (User)null;
                    entity.Connections.Remove(this);
                }
                this._User.Entity = value;
                if (value != null)
                {
                    value.Connections.Add(this);
                    this._UserId = new int?(value.UserId);
                }
                else
                    this._UserId = new int?();
            }
        }

        [Column(CanBeNull = false, Storage = "_Active", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = true, UpdateUnique = true)]
        public bool Active
        {
            get
            {
                return this._Active;
            }
        }

        [Column(CanBeNull = true, Storage = "_DeActivatedOn", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(IgnoreUniqueUpdateCheck = true, Internal = true, Unique = true, UpdateUnique = true)]
        public DateTime? DeActivatedOn
        {
            get
            {
                return this._DeActivatedOn;
            }
        }

        [Association(OtherKey = "ConnectionId", Storage = "_As2Connections", ThisKey = "ConnectionId")]
        public EntitySet<As2Connection> As2Connections
        {
            get
            {
                return this._As2Connections;
            }
        }

        [Association(OtherKey = "ConnectionId", Storage = "_EmailConnections", ThisKey = "ConnectionId")]
        public EntitySet<EmailConnection> EmailConnections
        {
            get
            {
                return this._EmailConnections;
            }
        }

        [Association(OtherKey = "ConnectionId", Storage = "_FtpConnections", ThisKey = "ConnectionId")]
        public EntitySet<FtpConnection> FtpConnections
        {
            get
            {
                return this._FtpConnections;
            }
        }

        [Association(OtherKey = "ConnectionId", Storage = "_AzureConnections", ThisKey = "ConnectionId")]
        public EntitySet<AzureConnection> AzureConnections
        {
            get
            {
                return this._AzureConnections;
            }
        }

        [Association(OtherKey = "ConnectionId", Storage = "_Pipelines", ThisKey = "ConnectionId")]
        public EntitySet<Pipeline> Pipelines
        {
            get
            {
                return this._Pipelines;
            }
        }

        public Connection()
        {
        }

        public Connection(DateTime TimeStamp, string ConnectionName, Broker Broker, ConnectionType ConnectionType)
        {
            this.TimeStamp = TimeStamp;
            this.ConnectionName = ConnectionName;
            this.Broker = Broker;
            this.ConnectionType = ConnectionType;
        }

        public void DeActivate(DateTime deActivatedOn)
        {
            this._Active = false;
            this._DeActivatedOn = new DateTime?(deActivatedOn);
        }

        public void Activate()
        {
            this._Active = true;
            this._DeActivatedOn = new DateTime?();
        }

        public override string ToString()
        {
            return this.ConnectionName.ToString();
        }
    }

    [BeterTable(TrackHistory = true)]
    [Table]
    public class ConnectionPipe : IBase
    {
        private bool _Active = true;
        private int _ConnectionPipeId;
        private DateTime _TimeStamp;
        private int _PipelineId;
        private EntityRef<Pipeline> _Pipeline;
        private int _AzureConnectionId;
        private EntityRef<AzureConnection> _AzureConnection;
        private DateTime? _DeActivatedOn;

        [Column(IsDbGenerated = true, IsPrimaryKey = true, Storage = "_ConnectionPipeId")]
        public int ConnectionPipeId
        {
            get
            {
                return this._ConnectionPipeId;
            }
            set
            {
                if (this._ConnectionPipeId == value)
                    return;
                this._ConnectionPipeId = value;
            }
        }

        [Column(CanBeNull = false, DbType = "datetime", Storage = "_TimeStamp", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Description = "When a record was inserted or last updated", Internal = true, Unique = false)]
        public DateTime TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
            }
        }

        [BeterColumn(Internal = false, Unique = true)]
        [Column(CanBeNull = false, DbType = "int", Storage = "_PipelineId", UpdateCheck = UpdateCheck.Never)]
        public int PipelineId
        {
            get
            {
                return this._PipelineId;
            }
            set
            {
                if (this._PipelineId == value)
                    return;
                if (this._Pipeline.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._PipelineId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "PipelineId", Storage = "_Pipeline", ThisKey = "PipelineId")]
        [BeterColumn(Internal = false, Unique = true)]
        public Pipeline Pipeline
        {
            get
            {
                return this._Pipeline.Entity;
            }
            set
            {
                Pipeline entity = this._Pipeline.Entity;
                if (entity == value && this._Pipeline.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._Pipeline.Entity = (Pipeline)null;
                    entity.ConnectionPipe.Remove(this);
                }
                this._Pipeline.Entity = value;
                if (value == null)
                    throw new Exception("'Pipeline' is a mandatory field and cannot be set to null.");
                value.ConnectionPipe.Add(this);
                this._PipelineId = value.PipelineId;
            }
        }

        [Column(CanBeNull = false, DbType = "int", Storage = "_AzureConnectionId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public int AzureConnectionId
        {
            get
            {
                return this._AzureConnectionId;
            }
            set
            {
                if (this._AzureConnectionId == value)
                    return;
                if (this._AzureConnection.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._AzureConnectionId = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Association(IsForeignKey = true, OtherKey = "AzureConnectionId", Storage = "_AzureConnection", ThisKey = "FTPConnectionId")]
        public AzureConnection AzureConnection
        {
            get
            {
                return this._AzureConnection.Entity;
            }
            set
            {
                AzureConnection entity = this._AzureConnection.Entity;
                if (entity == value && this._AzureConnection.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._AzureConnection.Entity = (AzureConnection)null;
                    entity.AzureConnectionAzurePipes.Remove(this);
                }
                this._AzureConnection.Entity = value;
                if (value == null)
                    throw new Exception("'FTP Connection' is a mandatory field and cannot be set to null.");
                value.AzureConnectionAzurePipes.Add(this);
                this._AzureConnectionId = value.AzureConnectionId;
            }
        }

        [Column(CanBeNull = false, Storage = "_Active", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = true, UpdateUnique = true)]
        public bool Active
        {
            get
            {
                return this._Active;
            }
        }

        [Column(CanBeNull = true, Storage = "_DeActivatedOn", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(IgnoreUniqueUpdateCheck = true, Internal = true, Unique = true, UpdateUnique = true)]
        public DateTime? DeActivatedOn
        {
            get
            {
                return this._DeActivatedOn;
            }
        }

        public ConnectionPipe()
        {
        }

        public ConnectionPipe(DateTime TimeStamp, Pipeline Pipeline, AzureConnection AzureConnection)
        {
            this.TimeStamp = TimeStamp;
            this.Pipeline = Pipeline;
            this.AzureConnection = AzureConnection;
        }

        public void DeActivate(DateTime deActivatedOn)
        {
            this._Active = false;
            this._DeActivatedOn = new DateTime?(deActivatedOn);
        }

        public void Activate()
        {
            this._Active = true;
            this._DeActivatedOn = new DateTime?();
        }

        public override string ToString()
        {
            return this.PipelineId.ToString();
        }
    }

    [BeterTable(TrackHistory = true)]
    [Table]
    public class Directory : IBase
    {
        private bool _Active = true;
        private EntitySet<Directory> _ParentDirectories = new EntitySet<Directory>();
        private EntitySet<FileDirectory> _FileDirectories = new EntitySet<FileDirectory>();
        private int _DirectoryId;
        private DateTime _TimeStamp;
        private int _BrokerId;
        private EntityRef<Broker> _Broker;
        private int? _ParentDirectoryId;
        private EntityRef<Directory> _ParentDirectory;
        private int _TagId;
        private EntityRef<Tag> _Tag;
        private bool? _EnableUpload;
        private int? _UserId;
        private EntityRef<User> _User;
        private DateTime? _DeActivatedOn;

        [Column(IsDbGenerated = true, IsPrimaryKey = true, Storage = "_DirectoryId")]
        public int DirectoryId
        {
            get
            {
                return this._DirectoryId;
            }
            set
            {
                if (this._DirectoryId == value)
                    return;
                this._DirectoryId = value;
            }
        }

        [BeterColumn(Description = "When a record was inserted or last updated", Internal = true, Unique = false)]
        [Column(CanBeNull = false, DbType = "datetime", Storage = "_TimeStamp", UpdateCheck = UpdateCheck.Never)]
        public DateTime TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
            }
        }

        [BeterColumn(Internal = false, Unique = true)]
        [Column(CanBeNull = false, DbType = "int", Storage = "_BrokerId", UpdateCheck = UpdateCheck.Never)]
        public int BrokerId
        {
            get
            {
                return this._BrokerId;
            }
            set
            {
                if (this._BrokerId == value)
                    return;
                if (this._Broker.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._BrokerId = value;
            }
        }

        [BeterColumn(Internal = false, Unique = true)]
        [Association(IsForeignKey = true, OtherKey = "BrokerId", Storage = "_Broker", ThisKey = "BrokerId")]
        public Broker Broker
        {
            get
            {
                return this._Broker.Entity;
            }
            set
            {
                Broker entity = this._Broker.Entity;
                if (entity == value && this._Broker.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._Broker.Entity = (Broker)null;
                    entity.Directories.Remove(this);
                }
                this._Broker.Entity = value;
                if (value == null)
                    throw new Exception("'Broker' is a mandatory field and cannot be set to null.");
                value.Directories.Add(this);
                this._BrokerId = value.BrokerId;
            }
        }

        [BeterColumn(Internal = false, Unique = true)]
        [Column(CanBeNull = true, DbType = "int", Storage = "_ParentDirectoryId", UpdateCheck = UpdateCheck.Never)]
        public int? ParentDirectoryId
        {
            get
            {
                return this._ParentDirectoryId;
            }
            set
            {
                int? nullable1 = this._ParentDirectoryId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._ParentDirectory.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._ParentDirectoryId = value;
            }
        }

        [BeterColumn(Internal = false, Unique = true)]
        [Association(IsForeignKey = true, OtherKey = "DirectoryId", Storage = "_ParentDirectory", ThisKey = "ParentDirectoryId")]
        public Directory ParentDirectory
        {
            get
            {
                return this._ParentDirectory.Entity;
            }
            set
            {
                Directory entity = this._ParentDirectory.Entity;
                if (entity == value && this._ParentDirectory.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._ParentDirectory.Entity = (Directory)null;
                    entity.ParentDirectories.Remove(this);
                }
                this._ParentDirectory.Entity = value;
                if (value != null)
                {
                    value.ParentDirectories.Add(this);
                    this._ParentDirectoryId = new int?(value.DirectoryId);
                }
                else
                    this._ParentDirectoryId = new int?();
            }
        }

        [BeterColumn(Internal = false, Unique = true)]
        [Column(CanBeNull = false, DbType = "int", Storage = "_TagId", UpdateCheck = UpdateCheck.Never)]
        public int TagId
        {
            get
            {
                return this._TagId;
            }
            set
            {
                if (this._TagId == value)
                    return;
                if (this._Tag.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._TagId = value;
            }
        }

        [BeterColumn(Internal = false, Unique = true)]
        [Association(IsForeignKey = true, OtherKey = "TagId", Storage = "_Tag", ThisKey = "TagId")]
        public Tag Tag
        {
            get
            {
                return this._Tag.Entity;
            }
            set
            {
                Tag entity = this._Tag.Entity;
                if (entity == value && this._Tag.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._Tag.Entity = (Tag)null;
                    entity.Directories.Remove(this);
                }
                this._Tag.Entity = value;
                if (value == null)
                    throw new Exception("'Tag' is a mandatory field and cannot be set to null.");
                value.Directories.Add(this);
                this._TagId = value.TagId;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "bit", Storage = "_EnableUpload", UpdateCheck = UpdateCheck.Never)]
        public bool? EnableUpload
        {
            get
            {
                return this._EnableUpload;
            }
            set
            {
                this._EnableUpload = value;
            }
        }

        [Column(CanBeNull = true, DbType = "int", Storage = "_UserId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = false)]
        public int? UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                int? nullable1 = this._UserId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._User.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._UserId = value;
            }
        }

        [BeterColumn(Description = "User who created the entry in the table", Internal = true, Unique = false)]
        [Association(IsForeignKey = true, OtherKey = "UserId", Storage = "_User", ThisKey = "UserId")]
        public User User
        {
            get
            {
                return this._User.Entity;
            }
            set
            {
                User entity = this._User.Entity;
                if (entity == value && this._User.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._User.Entity = (User)null;
                    entity.Directories.Remove(this);
                }
                this._User.Entity = value;
                if (value != null)
                {
                    value.Directories.Add(this);
                    this._UserId = new int?(value.UserId);
                }
                else
                    this._UserId = new int?();
            }
        }

        [Column(CanBeNull = false, Storage = "_Active", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = true, UpdateUnique = true)]
        public bool Active
        {
            get
            {
                return this._Active;
            }
        }

        [BeterColumn(IgnoreUniqueUpdateCheck = true, Internal = true, Unique = true, UpdateUnique = true)]
        [Column(CanBeNull = true, Storage = "_DeActivatedOn", UpdateCheck = UpdateCheck.Never)]
        public DateTime? DeActivatedOn
        {
            get
            {
                return this._DeActivatedOn;
            }
        }

        [Association(OtherKey = "ParentDirectoryId", Storage = "_ParentDirectories", ThisKey = "DirectoryId")]
        public EntitySet<Directory> ParentDirectories
        {
            get
            {
                return this._ParentDirectories;
            }
        }

        [Association(OtherKey = "DirectoryId", Storage = "_FileDirectories", ThisKey = "DirectoryId")]
        public EntitySet<FileDirectory> FileDirectories
        {
            get
            {
                return this._FileDirectories;
            }
        }

        public Directory()
        {
        }

        public Directory(DateTime TimeStamp, Broker Broker, Tag Tag)
        {
            this.TimeStamp = TimeStamp;
            this.Broker = Broker;
            this.Tag = Tag;
        }

        public IOrderedEnumerable<Directory> GetDirectories()
        {
            return Enumerable.OrderBy<Directory, string>(Enumerable.Where<Directory>((IEnumerable<Directory>)this.ParentDirectories, (Func<Directory, bool>)(u =>
            {
                if (u.Active)
                    return u.Tag.Active;
                return false;
            })), (Func<Directory, string>)(u => u.Tag.TagName));
        }

        public Directory GetDirectoryOrDefault(string name)
        {
            return Enumerable.SingleOrDefault<Directory>((IEnumerable<Directory>)this.ParentDirectories, (Func<Directory, bool>)(n =>
            {
                if (n.Active && n.Tag.Active)
                    return n.Tag.TagName.ToLower().Replace(" ", "") == name.ToLower().Replace(" ", "");
                return false;
            }));
        }

        public string GetDirectoryName()
        {
            return Format.RemoveSpacesAndSpecialCharacters(this.Tag.TagName);
        }

        public List<Tag> GetTags()
        {
            List<Tag> tags = new List<Tag>();
            this.GetTags(ref tags);
            return tags;
        }

        private void GetTags(ref List<Tag> tags)
        {
            tags.Insert(0, this.Tag);
            if (this.ParentDirectory == null || !this.ParentDirectory.Active)
                return;
            this.ParentDirectory.GetTags(ref tags);
        }

        public List<string> GetPath()
        {
            List<string> path = new List<string>();
            this.GetPath(ref path);
            return path;
        }

        public void GetPath(ref List<string> path)
        {
            if (!this.Tag.ExcludeFromFileDirectory.GetValueOrDefault())
                path.Insert(0, this.Tag.TagName);
            if (this.ParentDirectory == null || !this.ParentDirectory.Active)
                return;
            this.ParentDirectory.GetPath(ref path);
        }

        public IEnumerable<FileDirectory> GetFiles()
        {
            return (IEnumerable<FileDirectory>)Enumerable.OrderByDescending<FileDirectory, DateTime>(Enumerable.Where<FileDirectory>((IEnumerable<FileDirectory>)this.FileDirectories, (Func<FileDirectory, bool>)(u =>
            {
                if (u.Active)
                    return u.File.Active;
                return false;
            })), (Func<FileDirectory, DateTime>)(u => u.TimeStamp));
        }

        public bool ReachedRoot()
        {
            if (!this.Tag.ExcludeFromFileDirectory.GetValueOrDefault())
                return false;
            if (this.ParentDirectory == null)
                return true;
            if (this.ParentDirectory.Tag.ExcludeFromFileDirectory.GetValueOrDefault())
                return this.ParentDirectory.ReachedRoot();
            return false;
        }

        public void DeActivate(DateTime deActivatedOn)
        {
            this._Active = false;
            this._DeActivatedOn = new DateTime?(deActivatedOn);
        }

        public void Activate()
        {
            this._Active = true;
            this._DeActivatedOn = new DateTime?();
        }

        public override string ToString()
        {
            return this.TagId.ToString();
        }
    }

    [Table]
    [BeterTable(TrackHistory = true)]
    public class EmailConnection : IBase
    {
        private bool _Active = true;
        private EntitySet<EmailPipe> _EmailPipes = new EntitySet<EmailPipe>();
        private int _EmailConnectionId;
        private DateTime _TimeStamp;
        private int _ConnectionId;
        private EntityRef<Connection> _Connection;
        private string _EmailAddress;
        private string _Server;
        private string _Username;
        private string _Password;
        private int? _Port;
        private bool? _UseSsl;
        private int? _UserId;
        private EntityRef<User> _User;
        private DateTime? _DeActivatedOn;

        [Column(IsDbGenerated = true, IsPrimaryKey = true, Storage = "_EmailConnectionId")]
        public int EmailConnectionId
        {
            get
            {
                return this._EmailConnectionId;
            }
            set
            {
                if (this._EmailConnectionId == value)
                    return;
                this._EmailConnectionId = value;
            }
        }

        [Column(CanBeNull = false, DbType = "datetime", Storage = "_TimeStamp", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Description = "When a record was inserted or last updated", Internal = true, Unique = false)]
        public DateTime TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
            }
        }

        [Column(CanBeNull = false, DbType = "int", Storage = "_ConnectionId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = true)]
        public int ConnectionId
        {
            get
            {
                return this._ConnectionId;
            }
            set
            {
                if (this._ConnectionId == value)
                    return;
                if (this._Connection.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._ConnectionId = value;
            }
        }

        [BeterColumn(Internal = false, Unique = true)]
        [Association(IsForeignKey = true, OtherKey = "ConnectionId", Storage = "_Connection", ThisKey = "ConnectionId")]
        public Connection Connection
        {
            get
            {
                return this._Connection.Entity;
            }
            set
            {
                Connection entity = this._Connection.Entity;
                if (entity == value && this._Connection.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._Connection.Entity = (Connection)null;
                    entity.EmailConnections.Remove(this);
                }
                this._Connection.Entity = value;
                if (value == null)
                    throw new Exception("'Connection' is a mandatory field and cannot be set to null.");
                value.EmailConnections.Add(this);
                this._ConnectionId = value.ConnectionId;
            }
        }

        [Column(CanBeNull = false, DbType = "varchar(150)", Storage = "_EmailAddress", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public string EmailAddress
        {
            get
            {
                return this._EmailAddress;
            }
            set
            {
                this._EmailAddress = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "varchar(150)", Storage = "_Server", UpdateCheck = UpdateCheck.Never)]
        public string Server
        {
            get
            {
                return this._Server;
            }
            set
            {
                this._Server = value;
            }
        }

        [Column(CanBeNull = false, DbType = "varchar(50)", Storage = "_Username", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public string Username
        {
            get
            {
                return this._Username;
            }
            set
            {
                this._Username = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = false, DbType = "varchar(150)", Storage = "_Password", UpdateCheck = UpdateCheck.Never)]
        public string Password
        {
            get
            {
                return this._Password;
            }
            set
            {
                this._Password = value;
            }
        }

        [Column(CanBeNull = true, DbType = "int", Storage = "_Port", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public int? Port
        {
            get
            {
                return this._Port;
            }
            set
            {
                this._Port = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "bit", Storage = "_UseSsl", UpdateCheck = UpdateCheck.Never)]
        public bool? UseSsl
        {
            get
            {
                return this._UseSsl;
            }
            set
            {
                this._UseSsl = value;
            }
        }

        [Column(CanBeNull = true, DbType = "int", Storage = "_UserId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = false)]
        public int? UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                int? nullable1 = this._UserId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._User.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._UserId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "UserId", Storage = "_User", ThisKey = "UserId")]
        [BeterColumn(Description = "User who created the entry in the table", Internal = true, Unique = false)]
        public User User
        {
            get
            {
                return this._User.Entity;
            }
            set
            {
                User entity = this._User.Entity;
                if (entity == value && this._User.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._User.Entity = (User)null;
                    entity.EmailConnections.Remove(this);
                }
                this._User.Entity = value;
                if (value != null)
                {
                    value.EmailConnections.Add(this);
                    this._UserId = new int?(value.UserId);
                }
                else
                    this._UserId = new int?();
            }
        }

        [BeterColumn(Internal = true, Unique = true, UpdateUnique = true)]
        [Column(CanBeNull = false, Storage = "_Active", UpdateCheck = UpdateCheck.Never)]
        public bool Active
        {
            get
            {
                return this._Active;
            }
        }

        [Column(CanBeNull = true, Storage = "_DeActivatedOn", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(IgnoreUniqueUpdateCheck = true, Internal = true, Unique = true, UpdateUnique = true)]
        public DateTime? DeActivatedOn
        {
            get
            {
                return this._DeActivatedOn;
            }
        }

        [Association(OtherKey = "EmailConnectionId", Storage = "_EmailPipes", ThisKey = "EmailConnectionId")]
        public EntitySet<EmailPipe> EmailPipes
        {
            get
            {
                return this._EmailPipes;
            }
        }

        public EmailConnection()
        {
        }

        public EmailConnection(DateTime TimeStamp, Connection Connection, string EmailAddress, string Username, string Password)
        {
            this.TimeStamp = TimeStamp;
            this.Connection = Connection;
            this.EmailAddress = EmailAddress;
            this.Username = Username;
            this.Password = Password;
        }

        public void DeActivate(DateTime deActivatedOn)
        {
            this._Active = false;
            this._DeActivatedOn = new DateTime?(deActivatedOn);
        }

        public void Activate()
        {
            this._Active = true;
            this._DeActivatedOn = new DateTime?();
        }

        public override string ToString()
        {
            return this.ConnectionId.ToString();
        }
    }

    [Table]
    [BeterTable(TrackHistory = true)]
    public class EmailFile : IBase
    {
        private bool _Active = true;
        private int _EmailFileId;
        private DateTime _TimeStamp;
        private int _FileId;
        private EntityRef<File> _File;
        private string _EmailContent;
        private string _From;
        private string _To;
        private string _Subject;
        private DateTime? _EmailDate;
        private string _MessageUid;
        private int? _UserId;
        private EntityRef<User> _User;
        private DateTime? _DeActivatedOn;

        [Column(IsDbGenerated = true, IsPrimaryKey = true, Storage = "_EmailFileId")]
        public int EmailFileId
        {
            get
            {
                return this._EmailFileId;
            }
            set
            {
                if (this._EmailFileId == value)
                    return;
                this._EmailFileId = value;
            }
        }

        [BeterColumn(Description = "When a record was inserted or last updated", Internal = true, Unique = false)]
        [Column(CanBeNull = false, DbType = "datetime", Storage = "_TimeStamp", UpdateCheck = UpdateCheck.Never)]
        public DateTime TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = false, DbType = "int", Storage = "_FileId", UpdateCheck = UpdateCheck.Never)]
        public int FileId
        {
            get
            {
                return this._FileId;
            }
            set
            {
                if (this._FileId == value)
                    return;
                if (this._File.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._FileId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "FileId", Storage = "_File", ThisKey = "FileId")]
        [BeterColumn(Internal = false, Unique = false)]
        public File File
        {
            get
            {
                return this._File.Entity;
            }
            set
            {
                File entity = this._File.Entity;
                if (entity == value && this._File.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._File.Entity = (File)null;
                    entity.EmailFiles.Remove(this);
                }
                this._File.Entity = value;
                if (value == null)
                    throw new Exception("'File' is a mandatory field and cannot be set to null.");
                value.EmailFiles.Add(this);
                this._FileId = value.FileId;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "varchar(max)", Storage = "_EmailContent", UpdateCheck = UpdateCheck.Never)]
        public string EmailContent
        {
            get
            {
                return this._EmailContent;
            }
            set
            {
                this._EmailContent = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "varchar(150)", Storage = "_From", UpdateCheck = UpdateCheck.Never)]
        public string From
        {
            get
            {
                return this._From;
            }
            set
            {
                this._From = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "varchar(150)", Storage = "_To", UpdateCheck = UpdateCheck.Never)]
        public string To
        {
            get
            {
                return this._To;
            }
            set
            {
                this._To = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "varchar(500)", Storage = "_Subject", UpdateCheck = UpdateCheck.Never)]
        public string Subject
        {
            get
            {
                return this._Subject;
            }
            set
            {
                this._Subject = value;
            }
        }

        [Column(CanBeNull = true, DbType = "datetime", Storage = "_EmailDate", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public DateTime? EmailDate
        {
            get
            {
                return this._EmailDate;
            }
            set
            {
                this._EmailDate = value;
            }
        }

        [BeterColumn(Description = "Not the UID of mail server but the generated one", Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "varchar(500)", Storage = "_MessageUid", UpdateCheck = UpdateCheck.Never)]
        public string MessageUid
        {
            get
            {
                return this._MessageUid;
            }
            set
            {
                this._MessageUid = value;
            }
        }

        [Column(CanBeNull = true, DbType = "int", Storage = "_UserId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = false)]
        public int? UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                int? nullable1 = this._UserId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._User.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._UserId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "UserId", Storage = "_User", ThisKey = "UserId")]
        [BeterColumn(Description = "User who created the entry in the table", Internal = true, Unique = false)]
        public User User
        {
            get
            {
                return this._User.Entity;
            }
            set
            {
                User entity = this._User.Entity;
                if (entity == value && this._User.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._User.Entity = (User)null;
                    entity.EmailFiles.Remove(this);
                }
                this._User.Entity = value;
                if (value != null)
                {
                    value.EmailFiles.Add(this);
                    this._UserId = new int?(value.UserId);
                }
                else
                    this._UserId = new int?();
            }
        }

        [Column(CanBeNull = false, Storage = "_Active", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = false)]
        public bool Active
        {
            get
            {
                return this._Active;
            }
        }

        [Column(CanBeNull = true, Storage = "_DeActivatedOn", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = false)]
        public DateTime? DeActivatedOn
        {
            get
            {
                return this._DeActivatedOn;
            }
        }

        public EmailFile()
        {
        }

        public EmailFile(DateTime TimeStamp, File File)
        {
            this.TimeStamp = TimeStamp;
            this.File = File;
        }

        public void DeActivate(DateTime deActivatedOn)
        {
            this._Active = false;
            this._DeActivatedOn = new DateTime?(deActivatedOn);
        }

        public void Activate()
        {
            this._Active = true;
            this._DeActivatedOn = new DateTime?();
        }

        public override string ToString()
        {
            return this.FileId.ToString();
        }
    }

    [Table]
    [BeterTable(TrackHistory = true)]
    public class EmailPipe : IBase
    {
        private bool _Active = true;
        private int _EmailPipeId;
        private DateTime _TimeStamp;
        private int _PipelineId;
        private EntityRef<Pipeline> _Pipeline;
        private int _EmailConnectionId;
        private EntityRef<EmailConnection> _EmailConnection;
        private string _Folder;
        private string _From;
        private string _Subject;
        private int? _UserId;
        private EntityRef<User> _User;
        private DateTime? _DeActivatedOn;

        [Column(IsDbGenerated = true, IsPrimaryKey = true, Storage = "_EmailPipeId")]
        public int EmailPipeId
        {
            get
            {
                return this._EmailPipeId;
            }
            set
            {
                if (this._EmailPipeId == value)
                    return;
                this._EmailPipeId = value;
            }
        }

        [BeterColumn(Description = "When a record was inserted or last updated", Internal = true, Unique = false)]
        [Column(CanBeNull = false, DbType = "datetime", Storage = "_TimeStamp", UpdateCheck = UpdateCheck.Never)]
        public DateTime TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
            }
        }

        [Column(CanBeNull = false, DbType = "int", Storage = "_PipelineId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = true)]
        public int PipelineId
        {
            get
            {
                return this._PipelineId;
            }
            set
            {
                if (this._PipelineId == value)
                    return;
                if (this._Pipeline.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._PipelineId = value;
            }
        }

        [BeterColumn(Internal = false, Unique = true)]
        [Association(IsForeignKey = true, OtherKey = "PipelineId", Storage = "_Pipeline", ThisKey = "PipelineId")]
        public Pipeline Pipeline
        {
            get
            {
                return this._Pipeline.Entity;
            }
            set
            {
                Pipeline entity = this._Pipeline.Entity;
                if (entity == value && this._Pipeline.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._Pipeline.Entity = (Pipeline)null;
                    entity.EmailPipes.Remove(this);
                }
                this._Pipeline.Entity = value;
                if (value == null)
                    throw new Exception("'Pipeline' is a mandatory field and cannot be set to null.");
                value.EmailPipes.Add(this);
                this._PipelineId = value.PipelineId;
            }
        }

        [Column(CanBeNull = false, DbType = "int", Storage = "_EmailConnectionId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public int EmailConnectionId
        {
            get
            {
                return this._EmailConnectionId;
            }
            set
            {
                if (this._EmailConnectionId == value)
                    return;
                if (this._EmailConnection.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._EmailConnectionId = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Association(IsForeignKey = true, OtherKey = "EmailConnectionId", Storage = "_EmailConnection", ThisKey = "EmailConnectionId")]
        public EmailConnection EmailConnection
        {
            get
            {
                return this._EmailConnection.Entity;
            }
            set
            {
                EmailConnection entity = this._EmailConnection.Entity;
                if (entity == value && this._EmailConnection.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._EmailConnection.Entity = (EmailConnection)null;
                    entity.EmailPipes.Remove(this);
                }
                this._EmailConnection.Entity = value;
                if (value == null)
                    throw new Exception("'Email Connection' is a mandatory field and cannot be set to null.");
                value.EmailPipes.Add(this);
                this._EmailConnectionId = value.EmailConnectionId;
            }
        }

        [BeterColumn(Description = "Filter on email folder like Inbox", Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "varchar(250)", Storage = "_Folder", UpdateCheck = UpdateCheck.Never)]
        public string Folder
        {
            get
            {
                return this._Folder;
            }
            set
            {
                this._Folder = value;
            }
        }

        [BeterColumn(Description = "Filter on email from address", Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "varchar(250)", Storage = "_From", UpdateCheck = UpdateCheck.Never)]
        public string From
        {
            get
            {
                return this._From;
            }
            set
            {
                this._From = value;
            }
        }

        [Column(CanBeNull = true, DbType = "varchar(250)", Storage = "_Subject", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Description = "Filter on exact match of email subject", Internal = false, Unique = false)]
        public string Subject
        {
            get
            {
                return this._Subject;
            }
            set
            {
                this._Subject = value;
            }
        }

        [Column(CanBeNull = true, DbType = "int", Storage = "_UserId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = false)]
        public int? UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                int? nullable1 = this._UserId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._User.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._UserId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "UserId", Storage = "_User", ThisKey = "UserId")]
        [BeterColumn(Description = "User who created the entry in the table", Internal = true, Unique = false)]
        public User User
        {
            get
            {
                return this._User.Entity;
            }
            set
            {
                User entity = this._User.Entity;
                if (entity == value && this._User.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._User.Entity = (User)null;
                    entity.EmailPipes.Remove(this);
                }
                this._User.Entity = value;
                if (value != null)
                {
                    value.EmailPipes.Add(this);
                    this._UserId = new int?(value.UserId);
                }
                else
                    this._UserId = new int?();
            }
        }

        [Column(CanBeNull = false, Storage = "_Active", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = true, UpdateUnique = true)]
        public bool Active
        {
            get
            {
                return this._Active;
            }
        }

        [BeterColumn(IgnoreUniqueUpdateCheck = true, Internal = true, Unique = true, UpdateUnique = true)]
        [Column(CanBeNull = true, Storage = "_DeActivatedOn", UpdateCheck = UpdateCheck.Never)]
        public DateTime? DeActivatedOn
        {
            get
            {
                return this._DeActivatedOn;
            }
        }

        public EmailPipe()
        {
        }

        public EmailPipe(DateTime TimeStamp, Pipeline Pipeline, EmailConnection EmailConnection)
        {
            this.TimeStamp = TimeStamp;
            this.Pipeline = Pipeline;
            this.EmailConnection = EmailConnection;
        }

        public EmailFile AddEmail(DateTime receivedOn, string from, string to, string subject, string emailContent, string messageId)
        {
            File file = new File(DateTime.UtcNow, subject, FileType.Email);
            this.Pipeline.ApplyInboundSortRules(file, true);
            return new EmailFile(DateTime.UtcNow, file)
            {
                EmailDate = new DateTime?(receivedOn),
                From = from,
                To = to,
                EmailContent = emailContent,
                MessageUid = messageId
            };
        }

        public void DeActivate(DateTime deActivatedOn)
        {
            this._Active = false;
            this._DeActivatedOn = new DateTime?(deActivatedOn);
        }

        public void Activate()
        {
            this._Active = true;
            this._DeActivatedOn = new DateTime?();
        }

        public override string ToString()
        {
            return this.PipelineId.ToString();
        }
    }

    [BeterTable(TrackHistory = true)]
    [Table]
    public class Execution : IBase
    {
        private bool _Active = true;
        private EntitySet<ExecutionLog> _ExecutionLogs = new EntitySet<ExecutionLog>();
        private EntitySet<File> _Files = new EntitySet<File>();
        private EntitySet<FileExecution> _FileExecutions = new EntitySet<FileExecution>();
        private EntitySet<FileTag> _FileTags = new EntitySet<FileTag>();
        private int _ExecutionId;
        private DateTime _TimeStamp;
        private int _InterfaceId;
        private EntityRef<Interface> _Interface;
        private DateTime _Start;
        private DateTime? _End;
        private int? _FileId;
        private EntityRef<File> _File;
        private int? _UserId;
        private EntityRef<User> _User;
        private DateTime? _DeActivatedOn;

        [Column(IsDbGenerated = true, IsPrimaryKey = true, Storage = "_ExecutionId")]
        public int ExecutionId
        {
            get
            {
                return this._ExecutionId;
            }
            set
            {
                if (this._ExecutionId == value)
                    return;
                this._ExecutionId = value;
            }
        }

        [Column(CanBeNull = false, DbType = "datetime", Storage = "_TimeStamp", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Description = "When a record was inserted or last updated", Internal = true, Unique = false)]
        public DateTime TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
            }
        }

        [Column(CanBeNull = false, DbType = "int", Storage = "_InterfaceId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public int InterfaceId
        {
            get
            {
                return this._InterfaceId;
            }
            set
            {
                if (this._InterfaceId == value)
                    return;
                if (this._Interface.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._InterfaceId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "InterfaceId", Storage = "_Interface", ThisKey = "InterfaceId")]
        [BeterColumn(Internal = false, Unique = false)]
        public Interface Interface
        {
            get
            {
                return this._Interface.Entity;
            }
            set
            {
                Interface entity = this._Interface.Entity;
                if (entity == value && this._Interface.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._Interface.Entity = (Interface)null;
                    entity.Executions.Remove(this);
                }
                this._Interface.Entity = value;
                if (value == null)
                    throw new Exception("'Interface' is a mandatory field and cannot be set to null.");
                value.Executions.Add(this);
                this._InterfaceId = value.InterfaceId;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = false, DbType = "datetime", Storage = "_Start", UpdateCheck = UpdateCheck.Never)]
        public DateTime Start
        {
            get
            {
                return this._Start;
            }
            set
            {
                this._Start = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "datetime", Storage = "_End", UpdateCheck = UpdateCheck.Never)]
        public DateTime? End
        {
            get
            {
                return this._End;
            }
            set
            {
                this._End = value;
            }
        }

        [Column(CanBeNull = true, DbType = "int", Storage = "_FileId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public int? FileId
        {
            get
            {
                return this._FileId;
            }
            set
            {
                int? nullable1 = this._FileId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._File.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._FileId = value;
            }
        }

        [BeterColumn(Description = "TODO: Remove", Internal = false, Unique = false)]
        [Association(IsForeignKey = true, OtherKey = "FileId", Storage = "_File", ThisKey = "FileId")]
        public File File
        {
            get
            {
                return this._File.Entity;
            }
            set
            {
                File entity = this._File.Entity;
                if (entity == value && this._File.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._File.Entity = (File)null;
                    entity.Executions.Remove(this);
                }
                this._File.Entity = value;
                if (value != null)
                {
                    value.Executions.Add(this);
                    this._FileId = new int?(value.FileId);
                }
                else
                    this._FileId = new int?();
            }
        }

        [Column(CanBeNull = true, DbType = "int", Storage = "_UserId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = false)]
        public int? UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                int? nullable1 = this._UserId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._User.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._UserId = value;
            }
        }

        [BeterColumn(Description = "User who created the entry in the table", Internal = true, Unique = false)]
        [Association(IsForeignKey = true, OtherKey = "UserId", Storage = "_User", ThisKey = "UserId")]
        public User User
        {
            get
            {
                return this._User.Entity;
            }
            set
            {
                User entity = this._User.Entity;
                if (entity == value && this._User.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._User.Entity = (User)null;
                    entity.Executions.Remove(this);
                }
                this._User.Entity = value;
                if (value != null)
                {
                    value.Executions.Add(this);
                    this._UserId = new int?(value.UserId);
                }
                else
                    this._UserId = new int?();
            }
        }

        [BeterColumn(Internal = true, Unique = false)]
        [Column(CanBeNull = false, Storage = "_Active", UpdateCheck = UpdateCheck.Never)]
        public bool Active
        {
            get
            {
                return this._Active;
            }
        }

        [BeterColumn(Internal = true, Unique = false)]
        [Column(CanBeNull = true, Storage = "_DeActivatedOn", UpdateCheck = UpdateCheck.Never)]
        public DateTime? DeActivatedOn
        {
            get
            {
                return this._DeActivatedOn;
            }
        }

        [Association(OtherKey = "ExecutionId", Storage = "_ExecutionLogs", ThisKey = "ExecutionId")]
        public EntitySet<ExecutionLog> ExecutionLogs
        {
            get
            {
                return this._ExecutionLogs;
            }
        }

        [Association(OtherKey = "ExecutionId", Storage = "_Files", ThisKey = "ExecutionId")]
        public EntitySet<File> Files
        {
            get
            {
                return this._Files;
            }
        }

        [Association(OtherKey = "ExecutionId", Storage = "_FileExecutions", ThisKey = "ExecutionId")]
        public EntitySet<FileExecution> FileExecutions
        {
            get
            {
                return this._FileExecutions;
            }
        }

        [Association(OtherKey = "ExecutionId", Storage = "_FileTags", ThisKey = "ExecutionId")]
        public EntitySet<FileTag> FileTags
        {
            get
            {
                return this._FileTags;
            }
        }

        public Execution()
        {
        }

        public Execution(DateTime TimeStamp, Interface Interface, DateTime Start)
        {
            this.TimeStamp = TimeStamp;
            this.Interface = Interface;
            this.Start = Start;
        }

        public void AddLogHandler(Logger logger)
        {
            logger.EventLogged -= new Logger.LogHandler(this.Log);
            logger.EventLogged += new Logger.LogHandler(this.Log);
        }

        public void RemoveLogHandler(Logger logger)
        {
            logger.EventLogged -= new Logger.LogHandler(this.Log);
        }

        private void Log(string msg, LogEvent e)
        {
            ExecutionLog executionLog = new ExecutionLog(DateTime.UtcNow, this, (BevChain.Integration.Portable.LogType)e.LogType);
            if (msg.Length > 8000)
                executionLog.Message = msg.Substring(0, 8000);
            else
                executionLog.Message = msg;
        }

        public void DeActivate(DateTime deActivatedOn)
        {
            this._Active = false;
            this._DeActivatedOn = new DateTime?(deActivatedOn);
        }

        public void Activate()
        {
            this._Active = true;
            this._DeActivatedOn = new DateTime?();
        }

        public override string ToString()
        {
            return this.InterfaceId.ToString();
        }
    }

    [BeterTable(TrackHistory = true)]
    [Table]
    public class ExecutionLog : IBase
    {
        private bool _Active = true;
        private int _ExecutionLogId;
        private DateTime _TimeStamp;
        private int _ExecutionId;
        private EntityRef<Execution> _Execution;
        private BevChain.Integration.Portable.LogType _LogType;
        private string _Message;
        private int? _UserId;
        private EntityRef<User> _User;
        private DateTime? _DeActivatedOn;

        [Column(IsDbGenerated = true, IsPrimaryKey = true, Storage = "_ExecutionLogId")]
        public int ExecutionLogId
        {
            get
            {
                return this._ExecutionLogId;
            }
            set
            {
                if (this._ExecutionLogId == value)
                    return;
                this._ExecutionLogId = value;
            }
        }

        [Column(CanBeNull = false, DbType = "datetime", Storage = "_TimeStamp", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Description = "When a record was inserted or last updated", Internal = true, Unique = false)]
        public DateTime TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = false, DbType = "int", Storage = "_ExecutionId", UpdateCheck = UpdateCheck.Never)]
        public int ExecutionId
        {
            get
            {
                return this._ExecutionId;
            }
            set
            {
                if (this._ExecutionId == value)
                    return;
                if (this._Execution.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._ExecutionId = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Association(IsForeignKey = true, OtherKey = "ExecutionId", Storage = "_Execution", ThisKey = "ExecutionId")]
        public Execution Execution
        {
            get
            {
                return this._Execution.Entity;
            }
            set
            {
                Execution entity = this._Execution.Entity;
                if (entity == value && this._Execution.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._Execution.Entity = (Execution)null;
                    entity.ExecutionLogs.Remove(this);
                }
                this._Execution.Entity = value;
                if (value == null)
                    throw new Exception("'Execution' is a mandatory field and cannot be set to null.");
                value.ExecutionLogs.Add(this);
                this._ExecutionId = value.ExecutionId;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = false, DbType = "int", Storage = "_LogType", UpdateCheck = UpdateCheck.Never)]
        public BevChain.Integration.Portable.LogType LogType
        {
            get
            {
                return this._LogType;
            }
            set
            {
                this._LogType = value;
            }
        }

        [Column(CanBeNull = true, DbType = "varchar(8000)", Storage = "_Message", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public string Message
        {
            get
            {
                return this._Message;
            }
            set
            {
                this._Message = value;
            }
        }

        [Column(CanBeNull = true, DbType = "int", Storage = "_UserId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = false)]
        public int? UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                int? nullable1 = this._UserId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._User.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._UserId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "UserId", Storage = "_User", ThisKey = "UserId")]
        [BeterColumn(Description = "User who created the entry in the table", Internal = true, Unique = false)]
        public User User
        {
            get
            {
                return this._User.Entity;
            }
            set
            {
                User entity = this._User.Entity;
                if (entity == value && this._User.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._User.Entity = (User)null;
                    entity.ExecutionLogs.Remove(this);
                }
                this._User.Entity = value;
                if (value != null)
                {
                    value.ExecutionLogs.Add(this);
                    this._UserId = new int?(value.UserId);
                }
                else
                    this._UserId = new int?();
            }
        }

        [BeterColumn(Internal = true, Unique = false)]
        [Column(CanBeNull = false, Storage = "_Active", UpdateCheck = UpdateCheck.Never)]
        public bool Active
        {
            get
            {
                return this._Active;
            }
        }

        [Column(CanBeNull = true, Storage = "_DeActivatedOn", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = false)]
        public DateTime? DeActivatedOn
        {
            get
            {
                return this._DeActivatedOn;
            }
        }

        public ExecutionLog()
        {
        }

        public ExecutionLog(DateTime TimeStamp, Execution Execution, BevChain.Integration.Portable.LogType LogType)
        {
            this.TimeStamp = TimeStamp;
            this.Execution = Execution;
            this.LogType = LogType;
        }

        public void DeActivate(DateTime deActivatedOn)
        {
            this._Active = false;
            this._DeActivatedOn = new DateTime?(deActivatedOn);
        }

        public void Activate()
        {
            this._Active = true;
            this._DeActivatedOn = new DateTime?();
        }
    }

    [Table]
    [BeterTable(Description = "Data transmitted thru a pipeline (message) or stored in a file pool", TrackHistory = true)]
    public class File : IBase
    {
        private bool _Active = true;
        private EntitySet<FileExecution> _FileExecutions = new EntitySet<FileExecution>();
        private EntitySet<As2Log> _As2Logs = new EntitySet<As2Log>();
        private EntitySet<BinaryFile> _BinaryFiles = new EntitySet<BinaryFile>();
        private EntitySet<EmailFile> _EmailFiles = new EntitySet<EmailFile>();
        private EntitySet<Execution> _Executions = new EntitySet<Execution>();
        private EntitySet<File> _SourceFiles = new EntitySet<File>();
        private EntitySet<FileDirectory> _FileDirectories = new EntitySet<FileDirectory>();
        private EntitySet<FileTag> _FileTags = new EntitySet<FileTag>();
        private EntitySet<OutboundTask> _OutboundTasks = new EntitySet<OutboundTask>();
        private EntitySet<ProcessTask> _ProcessTasks = new EntitySet<ProcessTask>();
        private EntitySet<TextFile> _TextFiles = new EntitySet<TextFile>();
        private EntitySet<XmlFile> _XmlFiles = new EntitySet<XmlFile>();
        private int _FileId;
        private DateTime _TimeStamp;
        private string _Filename;
        private int? _SourceFileId;
        private EntityRef<File> _SourceFile;
        private FileType _FileType;
        private bool? _Sorted;
        private DateTime? _SortedOn;
        private string _Comment;
        private int? _PipelineId;
        private EntityRef<Pipeline> _Pipeline;
        private int? _UserId;
        private EntityRef<User> _User;
        private int? _ExecutionId;
        private EntityRef<Execution> _Execution;
        private DateTime? _DeActivatedOn;

        [Column(IsDbGenerated = true, IsPrimaryKey = true, Storage = "_FileId")]
        public int FileId
        {
            get
            {
                return this._FileId;
            }
            set
            {
                if (this._FileId == value)
                    return;
                this._FileId = value;
            }
        }

        [BeterColumn(Description = "When a record was inserted or last updated", Internal = true, Unique = false)]
        [Column(CanBeNull = false, DbType = "datetime", Storage = "_TimeStamp", UpdateCheck = UpdateCheck.Never)]
        public DateTime TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
            }
        }

        [Column(CanBeNull = false, DbType = "varchar(250)", Storage = "_Filename", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public string Filename
        {
            get
            {
                return this._Filename;
            }
            set
            {
                this._Filename = value;
            }
        }

        [Column(CanBeNull = true, DbType = "int", Storage = "_SourceFileId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public int? SourceFileId
        {
            get
            {
                return this._SourceFileId;
            }
            set
            {
                int? nullable1 = this._SourceFileId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._SourceFile.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._SourceFileId = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Association(IsForeignKey = true, OtherKey = "FileId", Storage = "_SourceFile", ThisKey = "SourceFileId")]
        public File SourceFile
        {
            get
            {
                return this._SourceFile.Entity;
            }
            set
            {
                File entity = this._SourceFile.Entity;
                if (entity == value && this._SourceFile.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._SourceFile.Entity = (File)null;
                    entity.SourceFiles.Remove(this);
                }
                this._SourceFile.Entity = value;
                if (value != null)
                {
                    value.SourceFiles.Add(this);
                    this._SourceFileId = new int?(value.FileId);
                }
                else
                    this._SourceFileId = new int?();
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = false, DbType = "int", Storage = "_FileType", UpdateCheck = UpdateCheck.Never)]
        public FileType FileType
        {
            get
            {
                return this._FileType;
            }
            set
            {
                this._FileType = value;
            }
        }

        [Column(CanBeNull = true, DbType = "bit", Storage = "_Sorted", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Description = "Indicate if a file has been assigned to outbound and process tasks", Internal = false, Unique = false)]
        public bool? Sorted
        {
            get
            {
                return this._Sorted;
            }
            set
            {
                this._Sorted = value;
            }
        }

        [Column(CanBeNull = true, DbType = "datetime", Storage = "_SortedOn", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public DateTime? SortedOn
        {
            get
            {
                return this._SortedOn;
            }
            set
            {
                this._SortedOn = value;
            }
        }

        [BeterColumn(Description = "Used for exception string currently", Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "varchar(8000)", Storage = "_Comment", UpdateCheck = UpdateCheck.Never)]
        public string Comment
        {
            get
            {
                return this._Comment;
            }
            set
            {
                this._Comment = value;
            }
        }

        [Column(CanBeNull = true, DbType = "int", Storage = "_PipelineId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public int? PipelineId
        {
            get
            {
                return this._PipelineId;
            }
            set
            {
                int? nullable1 = this._PipelineId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._Pipeline.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._PipelineId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "PipelineId", Storage = "_Pipeline", ThisKey = "PipelineId")]
        [BeterColumn(Description = "If the file originated from a pipeline", Internal = false, Unique = false)]
        public Pipeline Pipeline
        {
            get
            {
                return this._Pipeline.Entity;
            }
            set
            {
                Pipeline entity = this._Pipeline.Entity;
                if (entity == value && this._Pipeline.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._Pipeline.Entity = (Pipeline)null;
                    entity.Files.Remove(this);
                }
                this._Pipeline.Entity = value;
                if (value != null)
                {
                    value.Files.Add(this);
                    this._PipelineId = new int?(value.PipelineId);
                }
                else
                    this._PipelineId = new int?();
            }
        }

        [Column(CanBeNull = true, DbType = "int", Storage = "_UserId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = false)]
        public int? UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                int? nullable1 = this._UserId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._User.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._UserId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "UserId", Storage = "_User", ThisKey = "UserId")]
        [BeterColumn(Description = "User who created the entry in the table", Internal = true, Unique = false)]
        public User User
        {
            get
            {
                return this._User.Entity;
            }
            set
            {
                User entity = this._User.Entity;
                if (entity == value && this._User.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._User.Entity = (User)null;
                    entity.Files.Remove(this);
                }
                this._User.Entity = value;
                if (value != null)
                {
                    value.Files.Add(this);
                    this._UserId = new int?(value.UserId);
                }
                else
                    this._UserId = new int?();
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "int", Storage = "_ExecutionId", UpdateCheck = UpdateCheck.Never)]
        public int? ExecutionId
        {
            get
            {
                return this._ExecutionId;
            }
            set
            {
                int? nullable1 = this._ExecutionId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._Execution.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._ExecutionId = value;
            }
        }

        [BeterColumn(Description = "If the file originated from an interface execution", Internal = false, Unique = false)]
        [Association(IsForeignKey = true, OtherKey = "ExecutionId", Storage = "_Execution", ThisKey = "ExecutionId")]
        public Execution Execution
        {
            get
            {
                return this._Execution.Entity;
            }
            set
            {
                Execution entity = this._Execution.Entity;
                if (entity == value && this._Execution.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._Execution.Entity = (Execution)null;
                    entity.Files.Remove(this);
                }
                this._Execution.Entity = value;
                if (value != null)
                {
                    value.Files.Add(this);
                    this._ExecutionId = new int?(value.ExecutionId);
                }
                else
                    this._ExecutionId = new int?();
            }
        }

        [BeterColumn(Internal = true, Unique = false)]
        [Column(CanBeNull = false, Storage = "_Active", UpdateCheck = UpdateCheck.Never)]
        public bool Active
        {
            get
            {
                return this._Active;
            }
        }

        [Column(CanBeNull = true, Storage = "_DeActivatedOn", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = false)]
        public DateTime? DeActivatedOn
        {
            get
            {
                return this._DeActivatedOn;
            }
        }

        [Association(OtherKey = "FileId", Storage = "_FileExecutions", ThisKey = "FileId")]
        public EntitySet<FileExecution> FileExecutions
        {
            get
            {
                return this._FileExecutions;
            }
        }

        [Association(OtherKey = "FileId", Storage = "_As2Logs", ThisKey = "FileId")]
        public EntitySet<As2Log> As2Logs
        {
            get
            {
                return this._As2Logs;
            }
        }

        [Association(OtherKey = "FileId", Storage = "_BinaryFiles", ThisKey = "FileId")]
        public EntitySet<BinaryFile> BinaryFiles
        {
            get
            {
                return this._BinaryFiles;
            }
        }

        [Association(OtherKey = "FileId", Storage = "_EmailFiles", ThisKey = "FileId")]
        public EntitySet<EmailFile> EmailFiles
        {
            get
            {
                return this._EmailFiles;
            }
        }

        [Association(OtherKey = "FileId", Storage = "_Executions", ThisKey = "FileId")]
        public EntitySet<Execution> Executions
        {
            get
            {
                return this._Executions;
            }
        }

        [Association(OtherKey = "SourceFileId", Storage = "_SourceFiles", ThisKey = "FileId")]
        public EntitySet<File> SourceFiles
        {
            get
            {
                return this._SourceFiles;
            }
        }

        [Association(OtherKey = "FileId", Storage = "_FileDirectories", ThisKey = "FileId")]
        public EntitySet<FileDirectory> FileDirectories
        {
            get
            {
                return this._FileDirectories;
            }
        }

        [Association(OtherKey = "FileId", Storage = "_FileTags", ThisKey = "FileId")]
        public EntitySet<FileTag> FileTags
        {
            get
            {
                return this._FileTags;
            }
        }

        [Association(OtherKey = "FileId", Storage = "_OutboundTasks", ThisKey = "FileId")]
        public EntitySet<OutboundTask> OutboundTasks
        {
            get
            {
                return this._OutboundTasks;
            }
        }

        [Association(OtherKey = "FileId", Storage = "_ProcessTasks", ThisKey = "FileId")]
        public EntitySet<ProcessTask> ProcessTasks
        {
            get
            {
                return this._ProcessTasks;
            }
        }

        [Association(OtherKey = "FileId", Storage = "_TextFiles", ThisKey = "FileId")]
        public EntitySet<TextFile> TextFiles
        {
            get
            {
                return this._TextFiles;
            }
        }

        [Association(OtherKey = "FileId", Storage = "_XmlFiles", ThisKey = "FileId")]
        public EntitySet<XmlFile> XmlFiles
        {
            get
            {
                return this._XmlFiles;
            }
        }

        public File(string filename, byte[] content, File sourceFile = null)
        {
            this.Filename = filename;
            this.TimeStamp = DateTime.UtcNow;
            this.SourceFile = sourceFile;
            this.AddFileContent(content, this.GetExtension());
        }

        public File(string filename, object content, File sourceFile = null)
        {
            this.Filename = filename;
            this.TimeStamp = DateTime.UtcNow;
            this.SourceFile = sourceFile;
            this.AddFileContent(content);
        }

        public File()
        {
        }

        public File(DateTime TimeStamp, string Filename, FileType FileType)
        {
            this.TimeStamp = TimeStamp;
            this.Filename = Filename;
            this.FileType = FileType;
        }

        public string GetFileNameWithoutExtension()
        {
            return Path.GetFileNameWithoutExtension(this.Filename);
        }

        public string GetExtension()
        {
            return Path.GetExtension(this.Filename).ToLower();
        }

        private void AddFileContent(byte[] content, string extension)
        {
            if (!string.IsNullOrEmpty(extension) && extension.StartsWith("."))
                extension = extension.Substring(1);
            if (extension == "xml")
            {
                this.FileType = FileType.Xml;
                new XmlFile(DateTime.UtcNow, this).XmlContent = XDocument.Load((Stream)new MemoryStream(content));
            }
            else if (typeof(BinaryFileType).IsEnumDefined((object)extension))
            {
                this.FileType = FileType.Binary;
                BinaryFile binaryFile = new BinaryFile(DateTime.UtcNow, this)
                {
                    BinaryFileType = new BinaryFileType?((BinaryFileType)Enum.Parse(typeof(BinaryFileType), extension)),
                    BinaryContent = content
                };
            }
            else
            {
                this.FileType = FileType.Text;
                TextFile textFile = new TextFile(DateTime.UtcNow, this);
                Encoding encoding = Encoding.Default;
                if (content.Length > 4)
                {
                    if ((int)content[0] == 239 && (int)content[1] == 187 && (int)content[2] == 191)
                        encoding = Encoding.UTF8;
                    else if ((int)content[0] == (int)byte.MaxValue && (int)content[1] == 254 && ((int)content[2] == 0 && (int)content[3] == 0))
                    {
                        encoding = Encoding.UTF32;
                    }
                    else
                    {
                        if ((int)content[0] == 0 && (int)content[1] == 0 && ((int)content[2] == 254 && (int)content[2] == (int)byte.MaxValue))
                            throw new NotSupportedException("UTF-32 Big Endian");
                        if ((int)content[0] == (int)byte.MaxValue && (int)content[1] == 254)
                            encoding = Encoding.Unicode;
                        else if ((int)content[0] == 254 && (int)content[1] == (int)byte.MaxValue)
                            encoding = Encoding.BigEndianUnicode;
                    }
                }
                string @string = encoding.GetString(content);
                textFile.TextContent = @string;
            }
        }

        private void AddFileContent(object content)
        {
            if (content is XDocument)
            {
                this.FileType = FileType.Xml;
                XmlFile xmlFile = new XmlFile(DateTime.UtcNow, this)
                {
                    XmlContent = (XDocument)content
                };
                return;
            }
            if (content is XElement)
            {
                this.FileType = FileType.Xml;
                XmlFile xmlFile1 = new XmlFile(DateTime.UtcNow, this)
                {
                    XmlContent = new XDocument(content)
                };
                return;
            }
            if (content is byte[])
            {
                this.AddFileContent((byte[])content);
                return;
            }
            this.FileType = FileType.Text;
            TextFile textFile = new TextFile(DateTime.UtcNow, this)
            {
                TextContent = (string)content.ToString()
            };

        }

        public FileTag AddTag(Tag t, bool showFile, Pipeline p, Execution exe)
        {
            FileTag fileTag = Enumerable.SingleOrDefault<FileTag>((IEnumerable<FileTag>)this.FileTags, (Func<FileTag, bool>)(n =>
            {
                if (n.Active)
                    return n.TagId == t.TagId;
                return false;
            }));
            if (fileTag == null)
            {
                fileTag = new FileTag(DateTime.UtcNow, this, t);
                fileTag.ShowFile = new bool?(showFile);
                fileTag.Pipeline = p;
                fileTag.Execution = exe;
            }
            return fileTag;
        }

        public string GetTagValue(string parentTag)
        {
            FileTag fileTag = Enumerable.SingleOrDefault<FileTag>((IEnumerable<FileTag>)this.FileTags, (Func<FileTag, bool>)(n =>
            {
                if (n.Tag.ParentTag != null)
                    return n.Tag.ParentTag.TagName == parentTag;
                return false;
            }));
            if (fileTag != null)
                return fileTag.Tag.TagName;
            throw new Exception("File does not have a reference to tag '" + parentTag + "'.");
        }

        public string GetTagValueOrDefault(string parentTag)
        {
            FileTag fileTag = Enumerable.SingleOrDefault<FileTag>((IEnumerable<FileTag>)this.FileTags, (Func<FileTag, bool>)(n =>
            {
                if (n.Tag.ParentTag != null)
                    return n.Tag.ParentTag.TagName == parentTag;
                return false;
            }));
            if (fileTag != null)
                return fileTag.Tag.TagName;
            return (string)null;
        }

        public string GetFrom()
        {
            if (this.FileType == FileType.Email)
                return Enumerable.Single<EmailFile>((IEnumerable<EmailFile>)this.EmailFiles).From;
            if (this.SourceFile != null)
                return this.SourceFile.GetFrom();
            return (string)null;
        }

        public int GetFileSize()
        {
            switch (this.FileType)
            {
                case FileType.Text:
                    return Enumerable.Single<TextFile>((IEnumerable<TextFile>)this.TextFiles).TextContent.Length;
                case FileType.Xml:
                    return Enumerable.Single<XmlFile>((IEnumerable<XmlFile>)this.XmlFiles).XmlContent.ToString().Length;
                case FileType.Binary:
                    return Enumerable.Single<BinaryFile>((IEnumerable<BinaryFile>)this.BinaryFiles).BinaryContent.Length;
                default:
                    return 0;
            }
        }

        public byte[] GetFileContentAsByte()
        {
            switch (this.FileType)
            {
                case FileType.Text:
                    return Encoding.Default.GetBytes(Enumerable.Single<TextFile>((IEnumerable<TextFile>)this.TextFiles).TextContent);
                case FileType.Xml:
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (XmlWriter.Create((Stream)memoryStream))
                        {
                            Enumerable.Single<XmlFile>((IEnumerable<XmlFile>)this.XmlFiles).XmlContent.Save((Stream)memoryStream);
                            return memoryStream.ToArray();
                        }
                    }
                case FileType.Binary:
                    return Enumerable.Single<BinaryFile>((IEnumerable<BinaryFile>)this.BinaryFiles).BinaryContent;
                default:
                    throw new NotSupportedException(this.FileType.ToString());
            }
        }

        public string GetFileContentAsString()
        {
            switch (this.FileType)
            {
                case FileType.Text:
                    return Enumerable.Single<TextFile>((IEnumerable<TextFile>)this.TextFiles).TextContent;
                case FileType.Xml:
                    return Enumerable.Single<XmlFile>((IEnumerable<XmlFile>)this.XmlFiles).XmlContent.ToString();
                default:
                    throw new NotSupportedException(this.FileType.ToString());
            }
        }

        public Broker GetBroker()
        {
            if (this.Pipeline != null)
                return this.Pipeline.Connection.Broker;
            if (this.Execution != null)
                return this.Execution.Interface.Broker;
            return (Broker)null;
        }

        public void ApplySortRules(bool addDirectory = true)
        {
            Broker broker = this.GetBroker();
            if (broker != null)
            {
                foreach (Interface @interface in Enumerable.Where<Interface>((IEnumerable<Interface>)broker.Interfaces, (Func<Interface, bool>)(n =>
                {
                    if (n.Active)
                        return Enumerable.Any<InterfaceSortRule>((IEnumerable<InterfaceSortRule>)n.InterfaceSortRules, (Func<InterfaceSortRule, bool>)(x => x.Active));
                    return false;
                })))
                    @interface.ApplyInterfaceSortRules(this);
                foreach (Connection connection in Enumerable.Where<Connection>((IEnumerable<Connection>)broker.Connections, (Func<Connection, bool>)(n => n.Active)))
                {
                    foreach (Pipeline pipeline in Enumerable.Where<Pipeline>((IEnumerable<Pipeline>)connection.Pipelines, (Func<Pipeline, bool>)(n =>
                    {
                        if (n.Active)
                            return Enumerable.Any<OutboundPipeSortRule>((IEnumerable<OutboundPipeSortRule>)n.OutboundPipeSortRules, (Func<OutboundPipeSortRule, bool>)(x => x.Active));
                        return false;
                    })))
                        pipeline.ApplyOutboundSortRules(this);
                }
            }
            if (addDirectory)
            {
                foreach (FileTag fileTag in (IEnumerable<FileTag>)Enumerable.OrderByDescending<FileTag, DateTime>(Enumerable.Where<FileTag>((IEnumerable<FileTag>)this.FileTags, (Func<FileTag, bool>)(u =>
                {
                    if (u.Active)
                        return u.ShowFile.GetValueOrDefault();
                    return false;
                })), (Func<FileTag, DateTime>)(u => u.TimeStamp)))
                {
                    foreach (Directory directory in Enumerable.Where<Directory>((IEnumerable<Directory>)fileTag.Tag.Directories, (Func<Directory, bool>)(n => n.Active)))
                    {
                        Directory d = directory;
                        if (!Enumerable.Any<FileDirectory>((IEnumerable<FileDirectory>)this.FileDirectories, (Func<FileDirectory, bool>)(n =>
                        {
                            if (n.Active)
                                return n.DirectoryId == d.DirectoryId;
                            return false;
                        })) && fileTag.MatchParentDirectories(d))
                        {
                            FileDirectory fileDirectory = new FileDirectory(DateTime.UtcNow, this, d);
                        }
                    }
                }
            }
            this.Sorted = new bool?(true);
            this.SortedOn = new DateTime?(DateTime.UtcNow);
        }

        public List<List<string>> GetDirectoryPaths()
        {
            List<List<string>> list = new List<List<string>>();
            foreach (Directory directory in Enumerable.Select<FileDirectory, Directory>((IEnumerable<FileDirectory>)Enumerable.OrderByDescending<FileDirectory, DateTime>(Enumerable.Where<FileDirectory>((IEnumerable<FileDirectory>)this.FileDirectories, (Func<FileDirectory, bool>)(u => u.Active)), (Func<FileDirectory, DateTime>)(u => u.TimeStamp)), (Func<FileDirectory, Directory>)(u => u.Directory)))
                list.Add(directory.GetPath());
            return list;
        }

        public void DeActivate(DateTime deActivatedOn)
        {
            this._Active = false;
            this._DeActivatedOn = new DateTime?(deActivatedOn);
        }

        public void Activate()
        {
            this._Active = true;
            this._DeActivatedOn = new DateTime?();
        }

        public override string ToString()
        {
            return this.Filename.ToString();
        }
    }

    [Table]
    [BeterTable(TrackHistory = true)]
    public class FileDirectory : IBase
    {
        private bool _Active = true;
        private int _FileDirectoryId;
        private DateTime _TimeStamp;
        private int _FileId;
        private EntityRef<File> _File;
        private int _DirectoryId;
        private EntityRef<Directory> _Directory;
        private int? _UserId;
        private EntityRef<User> _User;
        private DateTime? _DeActivatedOn;

        [Column(IsDbGenerated = true, IsPrimaryKey = true, Storage = "_FileDirectoryId")]
        public int FileDirectoryId
        {
            get
            {
                return this._FileDirectoryId;
            }
            set
            {
                if (this._FileDirectoryId == value)
                    return;
                this._FileDirectoryId = value;
            }
        }

        [BeterColumn(Description = "When a record was inserted or last updated", Internal = true, Unique = false)]
        [Column(CanBeNull = false, DbType = "datetime", Storage = "_TimeStamp", UpdateCheck = UpdateCheck.Never)]
        public DateTime TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
            }
        }

        [BeterColumn(Internal = false, Unique = true)]
        [Column(CanBeNull = false, DbType = "int", Storage = "_FileId", UpdateCheck = UpdateCheck.Never)]
        public int FileId
        {
            get
            {
                return this._FileId;
            }
            set
            {
                if (this._FileId == value)
                    return;
                if (this._File.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._FileId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "FileId", Storage = "_File", ThisKey = "FileId")]
        [BeterColumn(Internal = false, Unique = true)]
        public File File
        {
            get
            {
                return this._File.Entity;
            }
            set
            {
                File entity = this._File.Entity;
                if (entity == value && this._File.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._File.Entity = (File)null;
                    entity.FileDirectories.Remove(this);
                }
                this._File.Entity = value;
                if (value == null)
                    throw new Exception("'File' is a mandatory field and cannot be set to null.");
                value.FileDirectories.Add(this);
                this._FileId = value.FileId;
            }
        }

        [Column(CanBeNull = false, DbType = "int", Storage = "_DirectoryId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = true)]
        public int DirectoryId
        {
            get
            {
                return this._DirectoryId;
            }
            set
            {
                if (this._DirectoryId == value)
                    return;
                if (this._Directory.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._DirectoryId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "DirectoryId", Storage = "_Directory", ThisKey = "DirectoryId")]
        [BeterColumn(Internal = false, Unique = true)]
        public Directory Directory
        {
            get
            {
                return this._Directory.Entity;
            }
            set
            {
                Directory entity = this._Directory.Entity;
                if (entity == value && this._Directory.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._Directory.Entity = (Directory)null;
                    entity.FileDirectories.Remove(this);
                }
                this._Directory.Entity = value;
                if (value == null)
                    throw new Exception("'Directory' is a mandatory field and cannot be set to null.");
                value.FileDirectories.Add(this);
                this._DirectoryId = value.DirectoryId;
            }
        }

        [Column(CanBeNull = true, DbType = "int", Storage = "_UserId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = false)]
        public int? UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                int? nullable1 = this._UserId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._User.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._UserId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "UserId", Storage = "_User", ThisKey = "UserId")]
        [BeterColumn(Description = "User who created the entry in the table", Internal = true, Unique = false)]
        public User User
        {
            get
            {
                return this._User.Entity;
            }
            set
            {
                User entity = this._User.Entity;
                if (entity == value && this._User.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._User.Entity = (User)null;
                    entity.FileDirectories.Remove(this);
                }
                this._User.Entity = value;
                if (value != null)
                {
                    value.FileDirectories.Add(this);
                    this._UserId = new int?(value.UserId);
                }
                else
                    this._UserId = new int?();
            }
        }

        [Column(CanBeNull = false, Storage = "_Active", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = true, UpdateUnique = true)]
        public bool Active
        {
            get
            {
                return this._Active;
            }
        }

        [Column(CanBeNull = true, Storage = "_DeActivatedOn", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(IgnoreUniqueUpdateCheck = true, Internal = true, Unique = true, UpdateUnique = true)]
        public DateTime? DeActivatedOn
        {
            get
            {
                return this._DeActivatedOn;
            }
        }

        public FileDirectory()
        {
        }

        public FileDirectory(DateTime TimeStamp, File File, Directory Directory)
        {
            this.TimeStamp = TimeStamp;
            this.File = File;
            this.Directory = Directory;
        }

        public void DeActivate(DateTime deActivatedOn)
        {
            this._Active = false;
            this._DeActivatedOn = new DateTime?(deActivatedOn);
        }

        public void Activate()
        {
            this._Active = true;
            this._DeActivatedOn = new DateTime?();
        }
    }

    [Table]
    [BeterTable(TrackHistory = true)]
    public class FileExecution : IBase
    {
        private bool _Active = true;
        private int _FileExecutionId;
        private DateTime _TimeStamp;
        private int _FileId;
        private EntityRef<File> _File;
        private int _ExecutionId;
        private EntityRef<Execution> _Execution;
        private int? _UserId;
        private EntityRef<User> _User;
        private DateTime? _DeActivatedOn;

        [Column(IsDbGenerated = true, IsPrimaryKey = true, Storage = "_FileExecutionId")]
        public int FileExecutionId
        {
            get
            {
                return this._FileExecutionId;
            }
            set
            {
                if (this._FileExecutionId == value)
                    return;
                this._FileExecutionId = value;
            }
        }

        [BeterColumn(Description = "When a record was inserted or last updated", Internal = true, Unique = false)]
        [Column(CanBeNull = false, DbType = "datetime", Storage = "_TimeStamp", UpdateCheck = UpdateCheck.Never)]
        public DateTime TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
            }
        }

        [BeterColumn(Internal = false, Unique = true)]
        [Column(CanBeNull = false, DbType = "int", Storage = "_FileId", UpdateCheck = UpdateCheck.Never)]
        public int FileId
        {
            get
            {
                return this._FileId;
            }
            set
            {
                if (this._FileId == value)
                    return;
                if (this._File.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._FileId = value;
            }
        }

        [BeterColumn(Internal = false, Unique = true)]
        [Association(IsForeignKey = true, OtherKey = "FileId", Storage = "_File", ThisKey = "FileId")]
        public File File
        {
            get
            {
                return this._File.Entity;
            }
            set
            {
                File entity = this._File.Entity;
                if (entity == value && this._File.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._File.Entity = (File)null;
                    entity.FileExecutions.Remove(this);
                }
                this._File.Entity = value;
                if (value == null)
                    throw new Exception("'File' is a mandatory field and cannot be set to null.");
                value.FileExecutions.Add(this);
                this._FileId = value.FileId;
            }
        }

        [BeterColumn(Internal = false, Unique = true)]
        [Column(CanBeNull = false, DbType = "int", Storage = "_ExecutionId", UpdateCheck = UpdateCheck.Never)]
        public int ExecutionId
        {
            get
            {
                return this._ExecutionId;
            }
            set
            {
                if (this._ExecutionId == value)
                    return;
                if (this._Execution.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._ExecutionId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "ExecutionId", Storage = "_Execution", ThisKey = "ExecutionId")]
        [BeterColumn(Internal = false, Unique = true)]
        public Execution Execution
        {
            get
            {
                return this._Execution.Entity;
            }
            set
            {
                Execution entity = this._Execution.Entity;
                if (entity == value && this._Execution.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._Execution.Entity = (Execution)null;
                    entity.FileExecutions.Remove(this);
                }
                this._Execution.Entity = value;
                if (value == null)
                    throw new Exception("'Execution' is a mandatory field and cannot be set to null.");
                value.FileExecutions.Add(this);
                this._ExecutionId = value.ExecutionId;
            }
        }

        [Column(CanBeNull = true, DbType = "int", Storage = "_UserId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = false)]
        public int? UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                int? nullable1 = this._UserId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._User.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._UserId = value;
            }
        }

        [BeterColumn(Description = "User who created the entry in the table", Internal = true, Unique = false)]
        [Association(IsForeignKey = true, OtherKey = "UserId", Storage = "_User", ThisKey = "UserId")]
        public User User
        {
            get
            {
                return this._User.Entity;
            }
            set
            {
                User entity = this._User.Entity;
                if (entity == value && this._User.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._User.Entity = (User)null;
                    entity.FileExecutions.Remove(this);
                }
                this._User.Entity = value;
                if (value != null)
                {
                    value.FileExecutions.Add(this);
                    this._UserId = new int?(value.UserId);
                }
                else
                    this._UserId = new int?();
            }
        }

        [Column(CanBeNull = false, Storage = "_Active", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = true, UpdateUnique = true)]
        public bool Active
        {
            get
            {
                return this._Active;
            }
        }

        [BeterColumn(IgnoreUniqueUpdateCheck = true, Internal = true, Unique = true, UpdateUnique = true)]
        [Column(CanBeNull = true, Storage = "_DeActivatedOn", UpdateCheck = UpdateCheck.Never)]
        public DateTime? DeActivatedOn
        {
            get
            {
                return this._DeActivatedOn;
            }
        }

        public FileExecution()
        {
        }

        public FileExecution(DateTime TimeStamp, File File, Execution Execution)
        {
            this.TimeStamp = TimeStamp;
            this.File = File;
            this.Execution = Execution;
        }

        public void DeActivate(DateTime deActivatedOn)
        {
            this._Active = false;
            this._DeActivatedOn = new DateTime?(deActivatedOn);
        }

        public void Activate()
        {
            this._Active = true;
            this._DeActivatedOn = new DateTime?();
        }
    }

    [Table]
    [BeterTable(TrackHistory = true)]
    public class FileTag : IBase
    {
        private bool _Active = true;
        private EntitySet<FileTag> _ParentFileTags = new EntitySet<FileTag>();
        private int _FileTagId;
        private DateTime _TimeStamp;
        private int _FileId;
        private EntityRef<File> _File;
        private int? _ParentFileTagId;
        private EntityRef<FileTag> _ParentFileTag;
        private int _TagId;
        private EntityRef<Tag> _Tag;
        private bool? _ShowFile;
        private int? _PipelineId;
        private EntityRef<Pipeline> _Pipeline;
        private int? _ExecutionId;
        private EntityRef<Execution> _Execution;
        private DateTime? _DeActivatedOn;
        private int? _UserId;
        private EntityRef<User> _User;

        [Column(IsDbGenerated = true, IsPrimaryKey = true, Storage = "_FileTagId")]
        public int FileTagId
        {
            get
            {
                return this._FileTagId;
            }
            set
            {
                if (this._FileTagId == value)
                    return;
                this._FileTagId = value;
            }
        }

        [Column(CanBeNull = false, DbType = "datetime", Storage = "_TimeStamp", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Description = "When a record was inserted or last updated", Internal = true, Unique = false)]
        public DateTime TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
            }
        }

        [Column(CanBeNull = false, DbType = "int", Storage = "_FileId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = true)]
        public int FileId
        {
            get
            {
                return this._FileId;
            }
            set
            {
                if (this._FileId == value)
                    return;
                if (this._File.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._FileId = value;
            }
        }

        [BeterColumn(Internal = false, Unique = true)]
        [Association(IsForeignKey = true, OtherKey = "FileId", Storage = "_File", ThisKey = "FileId")]
        public File File
        {
            get
            {
                return this._File.Entity;
            }
            set
            {
                File entity = this._File.Entity;
                if (entity == value && this._File.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._File.Entity = (File)null;
                    entity.FileTags.Remove(this);
                }
                this._File.Entity = value;
                if (value == null)
                    throw new Exception("'File' is a mandatory field and cannot be set to null.");
                value.FileTags.Add(this);
                this._FileId = value.FileId;
            }
        }

        [Column(CanBeNull = true, DbType = "int", Storage = "_ParentFileTagId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = true)]
        public int? ParentFileTagId
        {
            get
            {
                return this._ParentFileTagId;
            }
            set
            {
                int? nullable1 = this._ParentFileTagId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._ParentFileTag.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._ParentFileTagId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "FileTagId", Storage = "_ParentFileTag", ThisKey = "ParentFileTagId")]
        [BeterColumn(Internal = false, Unique = true)]
        public FileTag ParentFileTag
        {
            get
            {
                return this._ParentFileTag.Entity;
            }
            set
            {
                FileTag entity = this._ParentFileTag.Entity;
                if (entity == value && this._ParentFileTag.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._ParentFileTag.Entity = (FileTag)null;
                    entity.ParentFileTags.Remove(this);
                }
                this._ParentFileTag.Entity = value;
                if (value != null)
                {
                    value.ParentFileTags.Add(this);
                    this._ParentFileTagId = new int?(value.FileTagId);
                }
                else
                    this._ParentFileTagId = new int?();
            }
        }

        [BeterColumn(Internal = false, Unique = true)]
        [Column(CanBeNull = false, DbType = "int", Storage = "_TagId", UpdateCheck = UpdateCheck.Never)]
        public int TagId
        {
            get
            {
                return this._TagId;
            }
            set
            {
                if (this._TagId == value)
                    return;
                if (this._Tag.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._TagId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "TagId", Storage = "_Tag", ThisKey = "TagId")]
        [BeterColumn(Internal = false, Unique = true)]
        public Tag Tag
        {
            get
            {
                return this._Tag.Entity;
            }
            set
            {
                Tag entity = this._Tag.Entity;
                if (entity == value && this._Tag.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._Tag.Entity = (Tag)null;
                    entity.FileTags.Remove(this);
                }
                this._Tag.Entity = value;
                if (value == null)
                    throw new Exception("'Tag' is a mandatory field and cannot be set to null.");
                value.FileTags.Add(this);
                this._TagId = value.TagId;
            }
        }

        [BeterColumn(Description = "Show file on this level of the file directory", Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "bit", Storage = "_ShowFile", UpdateCheck = UpdateCheck.Never)]
        public bool? ShowFile
        {
            get
            {
                return this._ShowFile;
            }
            set
            {
                this._ShowFile = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "int", Storage = "_PipelineId", UpdateCheck = UpdateCheck.Never)]
        public int? PipelineId
        {
            get
            {
                return this._PipelineId;
            }
            set
            {
                int? nullable1 = this._PipelineId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._Pipeline.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._PipelineId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "PipelineId", Storage = "_Pipeline", ThisKey = "PipelineId")]
        [BeterColumn(Description = "If the tag was added by a pipeline", Internal = false, Unique = false)]
        public Pipeline Pipeline
        {
            get
            {
                return this._Pipeline.Entity;
            }
            set
            {
                Pipeline entity = this._Pipeline.Entity;
                if (entity == value && this._Pipeline.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._Pipeline.Entity = (Pipeline)null;
                    entity.FileTags.Remove(this);
                }
                this._Pipeline.Entity = value;
                if (value != null)
                {
                    value.FileTags.Add(this);
                    this._PipelineId = new int?(value.PipelineId);
                }
                else
                    this._PipelineId = new int?();
            }
        }

        [Column(CanBeNull = true, DbType = "int", Storage = "_ExecutionId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public int? ExecutionId
        {
            get
            {
                return this._ExecutionId;
            }
            set
            {
                int? nullable1 = this._ExecutionId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._Execution.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._ExecutionId = value;
            }
        }

        [BeterColumn(Description = "If the tag was added by an interface execution", Internal = false, Unique = false)]
        [Association(IsForeignKey = true, OtherKey = "ExecutionId", Storage = "_Execution", ThisKey = "ExecutionId")]
        public Execution Execution
        {
            get
            {
                return this._Execution.Entity;
            }
            set
            {
                Execution entity = this._Execution.Entity;
                if (entity == value && this._Execution.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._Execution.Entity = (Execution)null;
                    entity.FileTags.Remove(this);
                }
                this._Execution.Entity = value;
                if (value != null)
                {
                    value.FileTags.Add(this);
                    this._ExecutionId = new int?(value.ExecutionId);
                }
                else
                    this._ExecutionId = new int?();
            }
        }

        [Column(CanBeNull = false, Storage = "_Active", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = true, UpdateUnique = true)]
        public bool Active
        {
            get
            {
                return this._Active;
            }
        }

        [BeterColumn(IgnoreUniqueUpdateCheck = true, Internal = true, Unique = true, UpdateUnique = true)]
        [Column(CanBeNull = true, Storage = "_DeActivatedOn", UpdateCheck = UpdateCheck.Never)]
        public DateTime? DeActivatedOn
        {
            get
            {
                return this._DeActivatedOn;
            }
        }

        [BeterColumn(Internal = true, Unique = false)]
        [Column(CanBeNull = true, DbType = "int", Storage = "_UserId", UpdateCheck = UpdateCheck.Never)]
        public int? UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                int? nullable1 = this._UserId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._User.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._UserId = value;
            }
        }

        [BeterColumn(Description = "User who created the entry in the table", Internal = true, Unique = false)]
        [Association(IsForeignKey = true, OtherKey = "UserId", Storage = "_User", ThisKey = "UserId")]
        public User User
        {
            get
            {
                return this._User.Entity;
            }
            set
            {
                User entity = this._User.Entity;
                if (entity == value && this._User.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._User.Entity = (User)null;
                    entity.FileTags.Remove(this);
                }
                this._User.Entity = value;
                if (value != null)
                {
                    value.FileTags.Add(this);
                    this._UserId = new int?(value.UserId);
                }
                else
                    this._UserId = new int?();
            }
        }

        [Association(OtherKey = "ParentFileTagId", Storage = "_ParentFileTags", ThisKey = "FileTagId")]
        public EntitySet<FileTag> ParentFileTags
        {
            get
            {
                return this._ParentFileTags;
            }
        }

        public FileTag()
        {
        }

        public FileTag(DateTime TimeStamp, File File, Tag Tag)
        {
            this.TimeStamp = TimeStamp;
            this.File = File;
            this.Tag = Tag;
        }

        public FileTag AddTag(Tag t, bool showFile, Pipeline p, Execution exe)
        {
            FileTag fileTag = Enumerable.SingleOrDefault<FileTag>((IEnumerable<FileTag>)this.ParentFileTags, (Func<FileTag, bool>)(n =>
            {
                if (n.Active)
                    return n.TagId == t.TagId;
                return false;
            }));
            if (fileTag == null)
            {
                fileTag = new FileTag(DateTime.UtcNow, this.File, t);
                fileTag.ParentFileTag = this;
                fileTag.ShowFile = new bool?(showFile);
                fileTag.Pipeline = p;
                fileTag.Execution = exe;
            }
            return fileTag;
        }

        public bool MatchParentDirectories(Directory d)
        {
            if (this.TagId != d.TagId && !d.Tag.ExcludeFromFileDirectory.GetValueOrDefault())
                return false;
            if (this.ParentFileTag == null && d.ParentDirectory == null)
                return true;
            if (this.ParentFileTag == null)
                return d.ParentDirectory.ReachedRoot();
            if (d.ParentDirectory == null)
                return false;
            return this.ParentFileTag.MatchParentDirectories(d.ParentDirectory);
        }

        public void DeActivate(DateTime deActivatedOn)
        {
            this._Active = false;
            this._DeActivatedOn = new DateTime?(deActivatedOn);
        }

        public void Activate()
        {
            this._Active = true;
            this._DeActivatedOn = new DateTime?();
        }

        public override string ToString()
        {
            return this.FileId.ToString() + " | " + this.TagId.ToString();
        }
    }

    [Table]
    [BeterTable(TrackHistory = true)]
    public class FtpConnection : IBase
    {
        private bool _Active = true;
        private EntitySet<FtpPipe> _FTPConnectionFTPPipes = new EntitySet<FtpPipe>();
        private int _FtpConnectionId;
        private DateTime _TimeStamp;
        private int _ConnectionId;
        private EntityRef<Connection> _Connection;
        private string _Username;
        private string _Password;
        private int? _BrokerId;
        private EntityRef<Broker> _Broker;
        private int? _UserId;
        private EntityRef<User> _User;
        private DateTime? _DeActivatedOn;

        [Column(IsDbGenerated = true, IsPrimaryKey = true, Storage = "_FtpConnectionId")]
        public int FtpConnectionId
        {
            get
            {
                return this._FtpConnectionId;
            }
            set
            {
                if (this._FtpConnectionId == value)
                    return;
                this._FtpConnectionId = value;
            }
        }

        [Column(CanBeNull = false, DbType = "datetime", Storage = "_TimeStamp", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Description = "When a record was inserted or last updated", Internal = true, Unique = false)]
        public DateTime TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
            }
        }

        [Column(CanBeNull = false, DbType = "int", Storage = "_ConnectionId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = true)]
        public int ConnectionId
        {
            get
            {
                return this._ConnectionId;
            }
            set
            {
                if (this._ConnectionId == value)
                    return;
                if (this._Connection.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._ConnectionId = value;
            }
        }

        [BeterColumn(Internal = false, Unique = true)]
        [Association(IsForeignKey = true, OtherKey = "ConnectionId", Storage = "_Connection", ThisKey = "ConnectionId")]
        public Connection Connection
        {
            get
            {
                return this._Connection.Entity;
            }
            set
            {
                Connection entity = this._Connection.Entity;
                if (entity == value && this._Connection.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._Connection.Entity = (Connection)null;
                    entity.FtpConnections.Remove(this);
                }
                this._Connection.Entity = value;
                if (value == null)
                    throw new Exception("'Connection' is a mandatory field and cannot be set to null.");
                value.FtpConnections.Add(this);
                this._ConnectionId = value.ConnectionId;
            }
        }

        [Column(CanBeNull = true, DbType = "varchar(250)", Storage = "_Username", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public string Username
        {
            get
            {
                return this._Username;
            }
            set
            {
                this._Username = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "varchar(250)", Storage = "_Password", UpdateCheck = UpdateCheck.Never)]
        public string Password
        {
            get
            {
                return this._Password;
            }
            set
            {
                this._Password = value;
            }
        }

        [Column(CanBeNull = true, DbType = "int", Storage = "_BrokerId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public int? BrokerId
        {
            get
            {
                return this._BrokerId;
            }
            set
            {
                int? nullable1 = this._BrokerId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._Broker.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._BrokerId = value;
            }
        }

        [BeterColumn(Description = "? Broker is specified on the connection", Internal = false, Unique = false)]
        [Association(IsForeignKey = true, OtherKey = "BrokerId", Storage = "_Broker", ThisKey = "BrokerId")]
        public Broker Broker
        {
            get
            {
                return this._Broker.Entity;
            }
            set
            {
                Broker entity = this._Broker.Entity;
                if (entity == value && this._Broker.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._Broker.Entity = (Broker)null;
                    entity.FtpConnections.Remove(this);
                }
                this._Broker.Entity = value;
                if (value != null)
                {
                    value.FtpConnections.Add(this);
                    this._BrokerId = new int?(value.BrokerId);
                }
                else
                    this._BrokerId = new int?();
            }
        }

        [Column(CanBeNull = true, DbType = "int", Storage = "_UserId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = false)]
        public int? UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                int? nullable1 = this._UserId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._User.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._UserId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "UserId", Storage = "_User", ThisKey = "UserId")]
        [BeterColumn(Description = "User who created the entry in the table", Internal = true, Unique = false)]
        public User User
        {
            get
            {
                return this._User.Entity;
            }
            set
            {
                User entity = this._User.Entity;
                if (entity == value && this._User.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._User.Entity = (User)null;
                    entity.FtpConnections.Remove(this);
                }
                this._User.Entity = value;
                if (value != null)
                {
                    value.FtpConnections.Add(this);
                    this._UserId = new int?(value.UserId);
                }
                else
                    this._UserId = new int?();
            }
        }

        [BeterColumn(Internal = true, Unique = true, UpdateUnique = true)]
        [Column(CanBeNull = false, Storage = "_Active", UpdateCheck = UpdateCheck.Never)]
        public bool Active
        {
            get
            {
                return this._Active;
            }
        }

        [BeterColumn(IgnoreUniqueUpdateCheck = true, Internal = true, Unique = true, UpdateUnique = true)]
        [Column(CanBeNull = true, Storage = "_DeActivatedOn", UpdateCheck = UpdateCheck.Never)]
        public DateTime? DeActivatedOn
        {
            get
            {
                return this._DeActivatedOn;
            }
        }

        [Association(OtherKey = "FTPConnectionId", Storage = "_FTPConnectionFTPPipes", ThisKey = "FtpConnectionId")]
        public EntitySet<FtpPipe> FTPConnectionFTPPipes
        {
            get
            {
                return this._FTPConnectionFTPPipes;
            }
        }

        public FtpConnection()
        {
        }

        public FtpConnection(DateTime TimeStamp, Connection Connection)
        {
            this.TimeStamp = TimeStamp;
            this.Connection = Connection;
        }

        public void DeActivate(DateTime deActivatedOn)
        {
            this._Active = false;
            this._DeActivatedOn = new DateTime?(deActivatedOn);
        }

        public void Activate()
        {
            this._Active = true;
            this._DeActivatedOn = new DateTime?();
        }

        public override string ToString()
        {
            return this.ConnectionId.ToString();
        }
    }

    [DataServiceKey(new string[] { "PartitionKey", "RowKey" })]
    public class FtpEvent
    {
        public string PartitionKey { get; set; }

        public string RowKey { get; set; }

        public string ServerIp { get; set; }

        public string MessageType { get; set; }

        public string Message { get; set; }

        public string Parameter { get; set; }

        public FtpEvent()
        {
        }

        public FtpEvent(string partionKey, int rowKey, string serverIp, string messageType, string message)
        {
            this.PartitionKey = partionKey;
            this.RowKey = rowKey.ToString();
            this.ServerIp = serverIp;
            this.MessageType = messageType;
            this.Message = message.Substring(0, message.IndexOf(' '));
            this.Parameter = message.Substring(message.IndexOf(' ') + 1);
        }
    }

    [Table]
    [BeterTable(TrackHistory = true)]
    public class FtpLog : IBase
    {
        private bool _Active = true;
        private int _FtpLogId;
        private DateTime _TimeStamp;
        private int? _UserId;
        private EntityRef<User> _User;
        private DateTime? _DeActivatedOn;
        private string _RemoteIp;
        private string _ServerIp;
        private string _MessageType;
        private string _Message;
        private int? _FtpPipeId;
        private EntityRef<FtpPipe> _FtpPipe;

        [Column(IsDbGenerated = true, IsPrimaryKey = true, Storage = "_FtpLogId")]
        public int FtpLogId
        {
            get
            {
                return this._FtpLogId;
            }
            set
            {
                if (this._FtpLogId == value)
                    return;
                this._FtpLogId = value;
            }
        }

        [BeterColumn(Description = "When a record was inserted or last updated", Internal = true, Unique = false)]
        [Column(CanBeNull = false, DbType = "datetime", Storage = "_TimeStamp", UpdateCheck = UpdateCheck.Never)]
        public DateTime TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
            }
        }

        [BeterColumn(Internal = true, Unique = false)]
        [Column(CanBeNull = true, DbType = "int", Storage = "_UserId", UpdateCheck = UpdateCheck.Never)]
        public int? UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                int? nullable1 = this._UserId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._User.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._UserId = value;
            }
        }

        [BeterColumn(Description = "User who created the entry in the table", Internal = true, Unique = false)]
        [Association(IsForeignKey = true, OtherKey = "UserId", Storage = "_User", ThisKey = "UserId")]
        public User User
        {
            get
            {
                return this._User.Entity;
            }
            set
            {
                User entity = this._User.Entity;
                if (entity == value && this._User.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._User.Entity = (User)null;
                    entity.FtpLogs.Remove(this);
                }
                this._User.Entity = value;
                if (value != null)
                {
                    value.FtpLogs.Add(this);
                    this._UserId = new int?(value.UserId);
                }
                else
                    this._UserId = new int?();
            }
        }

        [BeterColumn(Internal = true, Unique = false)]
        [Column(CanBeNull = false, Storage = "_Active", UpdateCheck = UpdateCheck.Never)]
        public bool Active
        {
            get
            {
                return this._Active;
            }
        }

        [BeterColumn(Internal = true, Unique = false)]
        [Column(CanBeNull = true, Storage = "_DeActivatedOn", UpdateCheck = UpdateCheck.Never)]
        public DateTime? DeActivatedOn
        {
            get
            {
                return this._DeActivatedOn;
            }
        }

        [BeterColumn(Description = "IP:port", Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "varchar(50)", Storage = "_RemoteIp", UpdateCheck = UpdateCheck.Never)]
        public string RemoteIp
        {
            get
            {
                return this._RemoteIp;
            }
            set
            {
                this._RemoteIp = value;
            }
        }

        [Column(CanBeNull = true, DbType = "varchar(50)", Storage = "_ServerIp", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Description = "IP:port", Internal = false, Unique = false)]
        public string ServerIp
        {
            get
            {
                return this._ServerIp;
            }
            set
            {
                this._ServerIp = value;
            }
        }

        [Column(CanBeNull = true, DbType = "varchar(25)", Storage = "_MessageType", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Description = "Receive/Sent/Error", Internal = false, Unique = false)]
        public string MessageType
        {
            get
            {
                return this._MessageType;
            }
            set
            {
                this._MessageType = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "varchar(500)", Storage = "_Message", UpdateCheck = UpdateCheck.Never)]
        public string Message
        {
            get
            {
                return this._Message;
            }
            set
            {
                this._Message = value;
            }
        }

        [Column(CanBeNull = true, DbType = "int", Storage = "_FtpPipeId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public int? FtpPipeId
        {
            get
            {
                return this._FtpPipeId;
            }
            set
            {
                int? nullable1 = this._FtpPipeId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._FtpPipe.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._FtpPipeId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "FtpPipeId", Storage = "_FtpPipe", ThisKey = "FtpPipeId")]
        [BeterColumn(Description = "To track account", Internal = false, Unique = false)]
        public FtpPipe FtpPipe
        {
            get
            {
                return this._FtpPipe.Entity;
            }
            set
            {
                FtpPipe entity = this._FtpPipe.Entity;
                if (entity == value && this._FtpPipe.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._FtpPipe.Entity = (FtpPipe)null;
                    entity.FtpLogs.Remove(this);
                }
                this._FtpPipe.Entity = value;
                if (value != null)
                {
                    value.FtpLogs.Add(this);
                    this._FtpPipeId = new int?(value.FtpPipeId);
                }
                else
                    this._FtpPipeId = new int?();
            }
        }

        public FtpLog()
        {
        }

        public FtpLog(DateTime TimeStamp)
        {
            this.TimeStamp = TimeStamp;
        }

        public void DeActivate(DateTime deActivatedOn)
        {
            this._Active = false;
            this._DeActivatedOn = new DateTime?(deActivatedOn);
        }

        public void Activate()
        {
            this._Active = true;
            this._DeActivatedOn = new DateTime?();
        }
    }

    [BeterTable(TrackHistory = true)]
    [Table]
    public class FtpPipe : IBase
    {
        private bool _Active = true;
        private EntitySet<FtpLog> _FtpLogs = new EntitySet<FtpLog>();
        private int _FtpPipeId;
        private DateTime _TimeStamp;
        private int _PipelineId;
        private EntityRef<Pipeline> _Pipeline;
        private int _FTPConnectionId;
        private EntityRef<FtpConnection> _FTPConnection;
        private string _Username;
        private string _Password;
        private int? _UserId;
        private EntityRef<User> _User;
        private DateTime? _DeActivatedOn;

        [Column(IsDbGenerated = true, IsPrimaryKey = true, Storage = "_FtpPipeId")]
        public int FtpPipeId
        {
            get
            {
                return this._FtpPipeId;
            }
            set
            {
                if (this._FtpPipeId == value)
                    return;
                this._FtpPipeId = value;
            }
        }

        [Column(CanBeNull = false, DbType = "datetime", Storage = "_TimeStamp", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Description = "When a record was inserted or last updated", Internal = true, Unique = false)]
        public DateTime TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
            }
        }

        [BeterColumn(Internal = false, Unique = true)]
        [Column(CanBeNull = false, DbType = "int", Storage = "_PipelineId", UpdateCheck = UpdateCheck.Never)]
        public int PipelineId
        {
            get
            {
                return this._PipelineId;
            }
            set
            {
                if (this._PipelineId == value)
                    return;
                if (this._Pipeline.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._PipelineId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "PipelineId", Storage = "_Pipeline", ThisKey = "PipelineId")]
        [BeterColumn(Internal = false, Unique = true)]
        public Pipeline Pipeline
        {
            get
            {
                return this._Pipeline.Entity;
            }
            set
            {
                Pipeline entity = this._Pipeline.Entity;
                if (entity == value && this._Pipeline.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._Pipeline.Entity = (Pipeline)null;
                    entity.FTPPipes.Remove(this);
                }
                this._Pipeline.Entity = value;
                if (value == null)
                    throw new Exception("'Pipeline' is a mandatory field and cannot be set to null.");
                value.FTPPipes.Add(this);
                this._PipelineId = value.PipelineId;
            }
        }

        [Column(CanBeNull = false, DbType = "int", Storage = "_FTPConnectionId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public int FTPConnectionId
        {
            get
            {
                return this._FTPConnectionId;
            }
            set
            {
                if (this._FTPConnectionId == value)
                    return;
                if (this._FTPConnection.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._FTPConnectionId = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Association(IsForeignKey = true, OtherKey = "FtpConnectionId", Storage = "_FTPConnection", ThisKey = "FTPConnectionId")]
        public FtpConnection FTPConnection
        {
            get
            {
                return this._FTPConnection.Entity;
            }
            set
            {
                FtpConnection entity = this._FTPConnection.Entity;
                if (entity == value && this._FTPConnection.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._FTPConnection.Entity = (FtpConnection)null;
                    entity.FTPConnectionFTPPipes.Remove(this);
                }
                this._FTPConnection.Entity = value;
                if (value == null)
                    throw new Exception("'FTP Connection' is a mandatory field and cannot be set to null.");
                value.FTPConnectionFTPPipes.Add(this);
                this._FTPConnectionId = value.FtpConnectionId;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "varchar(250)", Storage = "_Username", UpdateCheck = UpdateCheck.Never)]
        public string Username
        {
            get
            {
                return this._Username;
            }
            set
            {
                this._Username = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "varchar(250)", Storage = "_Password", UpdateCheck = UpdateCheck.Never)]
        public string Password
        {
            get
            {
                return this._Password;
            }
            set
            {
                this._Password = value;
            }
        }

        [Column(CanBeNull = true, DbType = "int", Storage = "_UserId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = false)]
        public int? UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                int? nullable1 = this._UserId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._User.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._UserId = value;
            }
        }

        [BeterColumn(Description = "User who created the entry in the table", Internal = true, Unique = false)]
        [Association(IsForeignKey = true, OtherKey = "UserId", Storage = "_User", ThisKey = "UserId")]
        public User User
        {
            get
            {
                return this._User.Entity;
            }
            set
            {
                User entity = this._User.Entity;
                if (entity == value && this._User.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._User.Entity = (User)null;
                    entity.FTPPipes.Remove(this);
                }
                this._User.Entity = value;
                if (value != null)
                {
                    value.FTPPipes.Add(this);
                    this._UserId = new int?(value.UserId);
                }
                else
                    this._UserId = new int?();
            }
        }

        [Column(CanBeNull = false, Storage = "_Active", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = true, UpdateUnique = true)]
        public bool Active
        {
            get
            {
                return this._Active;
            }
        }

        [Column(CanBeNull = true, Storage = "_DeActivatedOn", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(IgnoreUniqueUpdateCheck = true, Internal = true, Unique = true, UpdateUnique = true)]
        public DateTime? DeActivatedOn
        {
            get
            {
                return this._DeActivatedOn;
            }
        }

        [Association(OtherKey = "FtpPipeId", Storage = "_FtpLogs", ThisKey = "FtpPipeId")]
        public EntitySet<FtpLog> FtpLogs
        {
            get
            {
                return this._FtpLogs;
            }
        }

        public FtpPipe()
        {
        }

        public FtpPipe(DateTime TimeStamp, Pipeline Pipeline, FtpConnection FTPConnection)
        {
            this.TimeStamp = TimeStamp;
            this.Pipeline = Pipeline;
            this.FTPConnection = FTPConnection;
        }

        public void DeActivate(DateTime deActivatedOn)
        {
            this._Active = false;
            this._DeActivatedOn = new DateTime?(deActivatedOn);
        }

        public void Activate()
        {
            this._Active = true;
            this._DeActivatedOn = new DateTime?();
        }

        public override string ToString()
        {
            return this.PipelineId.ToString();
        }
    }

    [BeterTable(TrackHistory = true)]
    [Table]
    public class HttpPipe : IBase
    {
        private bool _Active = true;
        private int _HttpPipeId;
        private DateTime _TimeStamp;
        private int _PipelineId;
        private EntityRef<Pipeline> _Pipeline;
        private string _HTTPIdentifier;
        private string _SubdomainIdentifier;
        private int? _UserId;
        private EntityRef<User> _User;
        private DateTime? _DeActivatedOn;

        [Column(IsDbGenerated = true, IsPrimaryKey = true, Storage = "_HttpPipeId")]
        public int HttpPipeId
        {
            get
            {
                return this._HttpPipeId;
            }
            set
            {
                if (this._HttpPipeId == value)
                    return;
                this._HttpPipeId = value;
            }
        }

        [BeterColumn(Description = "When a record was inserted or last updated", Internal = true, Unique = false)]
        [Column(CanBeNull = false, DbType = "datetime", Storage = "_TimeStamp", UpdateCheck = UpdateCheck.Never)]
        public DateTime TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
            }
        }

        [Column(CanBeNull = false, DbType = "int", Storage = "_PipelineId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = true)]
        public int PipelineId
        {
            get
            {
                return this._PipelineId;
            }
            set
            {
                if (this._PipelineId == value)
                    return;
                if (this._Pipeline.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._PipelineId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "PipelineId", Storage = "_Pipeline", ThisKey = "PipelineId")]
        [BeterColumn(Internal = false, Unique = true)]
        public Pipeline Pipeline
        {
            get
            {
                return this._Pipeline.Entity;
            }
            set
            {
                Pipeline entity = this._Pipeline.Entity;
                if (entity == value && this._Pipeline.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._Pipeline.Entity = (Pipeline)null;
                    entity.HttpPipes.Remove(this);
                }
                this._Pipeline.Entity = value;
                if (value == null)
                    throw new Exception("'Pipeline' is a mandatory field and cannot be set to null.");
                value.HttpPipes.Add(this);
                this._PipelineId = value.PipelineId;
            }
        }

        [Column(CanBeNull = true, DbType = "varchar(150)", Storage = "_HTTPIdentifier", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Description = "Eg. BrownBrothers", Internal = false, Unique = false)]
        public string HTTPIdentifier
        {
            get
            {
                return this._HTTPIdentifier;
            }
            set
            {
                this._HTTPIdentifier = value;
            }
        }

        [Column(CanBeNull = true, DbType = "varchar(250)", Storage = "_SubdomainIdentifier", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Description = "Eg. ItemMaster", Internal = false, Unique = false)]
        public string SubdomainIdentifier
        {
            get
            {
                return this._SubdomainIdentifier;
            }
            set
            {
                this._SubdomainIdentifier = value;
            }
        }

        [BeterColumn(Internal = true, Unique = false)]
        [Column(CanBeNull = true, DbType = "int", Storage = "_UserId", UpdateCheck = UpdateCheck.Never)]
        public int? UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                int? nullable1 = this._UserId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._User.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._UserId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "UserId", Storage = "_User", ThisKey = "UserId")]
        [BeterColumn(Description = "User who created the entry in the table", Internal = true, Unique = false)]
        public User User
        {
            get
            {
                return this._User.Entity;
            }
            set
            {
                User entity = this._User.Entity;
                if (entity == value && this._User.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._User.Entity = (User)null;
                    entity.HttpPipes.Remove(this);
                }
                this._User.Entity = value;
                if (value != null)
                {
                    value.HttpPipes.Add(this);
                    this._UserId = new int?(value.UserId);
                }
                else
                    this._UserId = new int?();
            }
        }

        [BeterColumn(Internal = true, Unique = true, UpdateUnique = true)]
        [Column(CanBeNull = false, Storage = "_Active", UpdateCheck = UpdateCheck.Never)]
        public bool Active
        {
            get
            {
                return this._Active;
            }
        }

        [BeterColumn(IgnoreUniqueUpdateCheck = true, Internal = true, Unique = true, UpdateUnique = true)]
        [Column(CanBeNull = true, Storage = "_DeActivatedOn", UpdateCheck = UpdateCheck.Never)]
        public DateTime? DeActivatedOn
        {
            get
            {
                return this._DeActivatedOn;
            }
        }

        public HttpPipe()
        {
        }

        public HttpPipe(DateTime TimeStamp, Pipeline Pipeline)
        {
            this.TimeStamp = TimeStamp;
            this.Pipeline = Pipeline;
        }

        public void DeActivate(DateTime deActivatedOn)
        {
            this._Active = false;
            this._DeActivatedOn = new DateTime?(deActivatedOn);
        }

        public void Activate()
        {
            this._Active = true;
            this._DeActivatedOn = new DateTime?();
        }
    }

    [BeterTable(TrackHistory = true)]
    [Table]
    public class InboundPipeTag : IBase
    {
        private bool _Active = true;
        private EntitySet<InboundPipeTagSortRule> _InboundPipeTagSortRules = new EntitySet<InboundPipeTagSortRule>();
        private int _InboundPipeTagId;
        private DateTime _TimeStamp;
        private int _PipelineId;
        private EntityRef<Pipeline> _Pipeline;
        private int _TagId;
        private EntityRef<Tag> _Tag;
        private int? _UserId;
        private EntityRef<User> _User;
        private DateTime? _DeActivatedOn;

        [Column(IsDbGenerated = true, IsPrimaryKey = true, Storage = "_InboundPipeTagId")]
        public int InboundPipeTagId
        {
            get
            {
                return this._InboundPipeTagId;
            }
            set
            {
                if (this._InboundPipeTagId == value)
                    return;
                this._InboundPipeTagId = value;
            }
        }

        [BeterColumn(Description = "When a record was inserted or last updated", Internal = true, Unique = false)]
        [Column(CanBeNull = false, DbType = "datetime", Storage = "_TimeStamp", UpdateCheck = UpdateCheck.Never)]
        public DateTime TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
            }
        }

        [BeterColumn(Internal = false, Unique = true)]
        [Column(CanBeNull = false, DbType = "int", Storage = "_PipelineId", UpdateCheck = UpdateCheck.Never)]
        public int PipelineId
        {
            get
            {
                return this._PipelineId;
            }
            set
            {
                if (this._PipelineId == value)
                    return;
                if (this._Pipeline.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._PipelineId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "PipelineId", Storage = "_Pipeline", ThisKey = "PipelineId")]
        [BeterColumn(Internal = false, Unique = true)]
        public Pipeline Pipeline
        {
            get
            {
                return this._Pipeline.Entity;
            }
            set
            {
                Pipeline entity = this._Pipeline.Entity;
                if (entity == value && this._Pipeline.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._Pipeline.Entity = (Pipeline)null;
                    entity.InboundPipeTags.Remove(this);
                }
                this._Pipeline.Entity = value;
                if (value == null)
                    throw new Exception("'Pipeline' is a mandatory field and cannot be set to null.");
                value.InboundPipeTags.Add(this);
                this._PipelineId = value.PipelineId;
            }
        }

        [Column(CanBeNull = false, DbType = "int", Storage = "_TagId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = true)]
        public int TagId
        {
            get
            {
                return this._TagId;
            }
            set
            {
                if (this._TagId == value)
                    return;
                if (this._Tag.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._TagId = value;
            }
        }

        [BeterColumn(Description = "Tag to be added", Internal = false, Unique = true)]
        [Association(IsForeignKey = true, OtherKey = "TagId", Storage = "_Tag", ThisKey = "TagId")]
        public Tag Tag
        {
            get
            {
                return this._Tag.Entity;
            }
            set
            {
                Tag entity = this._Tag.Entity;
                if (entity == value && this._Tag.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._Tag.Entity = (Tag)null;
                    entity.InboundPipeTags.Remove(this);
                }
                this._Tag.Entity = value;
                if (value == null)
                    throw new Exception("'Tag' is a mandatory field and cannot be set to null.");
                value.InboundPipeTags.Add(this);
                this._TagId = value.TagId;
            }
        }

        [Column(CanBeNull = true, DbType = "int", Storage = "_UserId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = false)]
        public int? UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                int? nullable1 = this._UserId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._User.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._UserId = value;
            }
        }

        [BeterColumn(Description = "User who created the entry in the table", Internal = true, Unique = false)]
        [Association(IsForeignKey = true, OtherKey = "UserId", Storage = "_User", ThisKey = "UserId")]
        public User User
        {
            get
            {
                return this._User.Entity;
            }
            set
            {
                User entity = this._User.Entity;
                if (entity == value && this._User.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._User.Entity = (User)null;
                    entity.InboundPipeTags.Remove(this);
                }
                this._User.Entity = value;
                if (value != null)
                {
                    value.InboundPipeTags.Add(this);
                    this._UserId = new int?(value.UserId);
                }
                else
                    this._UserId = new int?();
            }
        }

        [BeterColumn(Internal = true, Unique = true, UpdateUnique = true)]
        [Column(CanBeNull = false, Storage = "_Active", UpdateCheck = UpdateCheck.Never)]
        public bool Active
        {
            get
            {
                return this._Active;
            }
        }

        [Column(CanBeNull = true, Storage = "_DeActivatedOn", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(IgnoreUniqueUpdateCheck = true, Internal = true, Unique = true, UpdateUnique = true)]
        public DateTime? DeActivatedOn
        {
            get
            {
                return this._DeActivatedOn;
            }
        }

        [Association(OtherKey = "InboundPipeTagId", Storage = "_InboundPipeTagSortRules", ThisKey = "InboundPipeTagId")]
        public EntitySet<InboundPipeTagSortRule> InboundPipeTagSortRules
        {
            get
            {
                return this._InboundPipeTagSortRules;
            }
        }

        public InboundPipeTag()
        {
        }

        public InboundPipeTag(DateTime TimeStamp, Pipeline Pipeline, Tag Tag)
        {
            this.TimeStamp = TimeStamp;
            this.Pipeline = Pipeline;
            this.Tag = Tag;
        }

        public void DeActivate(DateTime deActivatedOn)
        {
            this._Active = false;
            this._DeActivatedOn = new DateTime?(deActivatedOn);
        }

        public void Activate()
        {
            this._Active = true;
            this._DeActivatedOn = new DateTime?();
        }
    }

    [BeterTable(TrackHistory = true)]
    [Table]
    public class InboundPipeTagSortRule : IBase, ISortRule
    {
        private bool _Active = true;
        private int _InboundPipeTagSortRuleId;
        private DateTime _TimeStamp;
        private int _InboundPipeTagId;
        private EntityRef<InboundPipeTag> _InboundPipeTag;
        private string _SortRuleName;
        private OperatorType _SortOperator;
        private ConditionType _SortCondition;
        private FieldType _SortFieldType;
        private string _SortValue;
        private string _SortValueTo;
        private FileType? _SortFileType;
        private int? _UserId;
        private EntityRef<User> _User;
        private DateTime? _DeActivatedOn;

        [Column(IsDbGenerated = true, IsPrimaryKey = true, Storage = "_InboundPipeTagSortRuleId")]
        public int InboundPipeTagSortRuleId
        {
            get
            {
                return this._InboundPipeTagSortRuleId;
            }
            set
            {
                if (this._InboundPipeTagSortRuleId == value)
                    return;
                this._InboundPipeTagSortRuleId = value;
            }
        }

        [BeterColumn(Description = "When a record was inserted or last updated", Internal = true, Unique = false)]
        [Column(CanBeNull = false, DbType = "datetime", Storage = "_TimeStamp", UpdateCheck = UpdateCheck.Never)]
        public DateTime TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = false, DbType = "int", Storage = "_InboundPipeTagId", UpdateCheck = UpdateCheck.Never)]
        public int InboundPipeTagId
        {
            get
            {
                return this._InboundPipeTagId;
            }
            set
            {
                if (this._InboundPipeTagId == value)
                    return;
                if (this._InboundPipeTag.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._InboundPipeTagId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "InboundPipeTagId", Storage = "_InboundPipeTag", ThisKey = "InboundPipeTagId")]
        [BeterColumn(Internal = false, Unique = false)]
        public InboundPipeTag InboundPipeTag
        {
            get
            {
                return this._InboundPipeTag.Entity;
            }
            set
            {
                InboundPipeTag entity = this._InboundPipeTag.Entity;
                if (entity == value && this._InboundPipeTag.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._InboundPipeTag.Entity = (InboundPipeTag)null;
                    entity.InboundPipeTagSortRules.Remove(this);
                }
                this._InboundPipeTag.Entity = value;
                if (value == null)
                    throw new Exception("'Inbound Pipe Tag' is a mandatory field and cannot be set to null.");
                value.InboundPipeTagSortRules.Add(this);
                this._InboundPipeTagId = value.InboundPipeTagId;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "varchar(50)", Storage = "_SortRuleName", UpdateCheck = UpdateCheck.Never)]
        public string SortRuleName
        {
            get
            {
                return this._SortRuleName;
            }
            set
            {
                this._SortRuleName = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = false, DbType = "int", Storage = "_SortOperator", UpdateCheck = UpdateCheck.Never)]
        public OperatorType SortOperator
        {
            get
            {
                return this._SortOperator;
            }
            set
            {
                this._SortOperator = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = false, DbType = "int", Storage = "_SortCondition", UpdateCheck = UpdateCheck.Never)]
        public ConditionType SortCondition
        {
            get
            {
                return this._SortCondition;
            }
            set
            {
                this._SortCondition = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = false, DbType = "int", Storage = "_SortFieldType", UpdateCheck = UpdateCheck.Never)]
        public FieldType SortFieldType
        {
            get
            {
                return this._SortFieldType;
            }
            set
            {
                this._SortFieldType = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = false, DbType = "varchar(50)", Storage = "_SortValue", UpdateCheck = UpdateCheck.Never)]
        public string SortValue
        {
            get
            {
                return this._SortValue;
            }
            set
            {
                this._SortValue = value;
            }
        }

        [Column(CanBeNull = true, DbType = "varchar(50)", Storage = "_SortValueTo", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public string SortValueTo
        {
            get
            {
                return this._SortValueTo;
            }
            set
            {
                this._SortValueTo = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "int", Storage = "_SortFileType", UpdateCheck = UpdateCheck.Never)]
        public FileType? SortFileType
        {
            get
            {
                return this._SortFileType;
            }
            set
            {
                this._SortFileType = value;
            }
        }

        [Column(CanBeNull = true, DbType = "int", Storage = "_UserId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = false)]
        public int? UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                int? nullable1 = this._UserId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._User.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._UserId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "UserId", Storage = "_User", ThisKey = "UserId")]
        [BeterColumn(Description = "User who created the entry in the table", Internal = true, Unique = false)]
        public User User
        {
            get
            {
                return this._User.Entity;
            }
            set
            {
                User entity = this._User.Entity;
                if (entity == value && this._User.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._User.Entity = (User)null;
                    entity.InboundPipeTagSortRules.Remove(this);
                }
                this._User.Entity = value;
                if (value != null)
                {
                    value.InboundPipeTagSortRules.Add(this);
                    this._UserId = new int?(value.UserId);
                }
                else
                    this._UserId = new int?();
            }
        }

        [BeterColumn(Internal = true, Unique = false)]
        [Column(CanBeNull = false, Storage = "_Active", UpdateCheck = UpdateCheck.Never)]
        public bool Active
        {
            get
            {
                return this._Active;
            }
        }

        [Column(CanBeNull = true, Storage = "_DeActivatedOn", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = false)]
        public DateTime? DeActivatedOn
        {
            get
            {
                return this._DeActivatedOn;
            }
        }

        public InboundPipeTagSortRule()
        {
        }

        public InboundPipeTagSortRule(DateTime TimeStamp, InboundPipeTag InboundPipeTag, OperatorType SortOperator, ConditionType SortCondition, FieldType SortFieldType, string SortValue)
        {
            this.TimeStamp = TimeStamp;
            this.InboundPipeTag = InboundPipeTag;
            this.SortOperator = SortOperator;
            this.SortCondition = SortCondition;
            this.SortFieldType = SortFieldType;
            this.SortValue = SortValue;
        }

        public void DeActivate(DateTime deActivatedOn)
        {
            this._Active = false;
            this._DeActivatedOn = new DateTime?(deActivatedOn);
        }

        public void Activate()
        {
            this._Active = true;
            this._DeActivatedOn = new DateTime?();
        }
    }

    [BeterTable(TrackHistory = true)]
    [Table]
    public class Interface : IBase
    {
        private bool _Active = true;
        private EntitySet<Execution> _Executions = new EntitySet<Execution>();
        private EntitySet<InterfaceSortRule> _InterfaceSortRules = new EntitySet<InterfaceSortRule>();
        private EntitySet<InterfaceRequest> _InterfaceRequests = new EntitySet<InterfaceRequest>();
        private EntitySet<OutboundInterfaceTag> _OutboundInterfaceTags = new EntitySet<OutboundInterfaceTag>();
        private EntitySet<ProcessTask> _ProcessTasks = new EntitySet<ProcessTask>();
        private int _InterfaceId;
        private DateTime _TimeStamp;
        private int _BrokerId;
        private EntityRef<Broker> _Broker;
        private string _InterfaceName;
        private string _InterfaceDescription;
        private int _ProcessBlockId;
        private EntityRef<ProcessBlock> _ProcessBlock;
        private ExecutionEnvironment _ExecutionEnvironment;
        private bool? _Running;
        private int? _ScheduleFactor;
        private ScheduleCycle? _ScheduleFrequency;
        private SendNotification _SendNotification;
        private string _NotificationEmail;
        private int? _MaximumExecutionTime;
        private int? _MaxBatch;
        private int? _UserId;
        private EntityRef<User> _User;
        private DateTime? _DeActivatedOn;

        [Column(IsDbGenerated = true, IsPrimaryKey = true, Storage = "_InterfaceId")]
        public int InterfaceId
        {
            get
            {
                return this._InterfaceId;
            }
            set
            {
                if (this._InterfaceId == value)
                    return;
                this._InterfaceId = value;
            }
        }

        [BeterColumn(Description = "When a record was inserted or last updated", Internal = true, Unique = false)]
        [Column(CanBeNull = false, DbType = "datetime", Storage = "_TimeStamp", UpdateCheck = UpdateCheck.Never)]
        public DateTime TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = false, DbType = "int", Storage = "_BrokerId", UpdateCheck = UpdateCheck.Never)]
        public int BrokerId
        {
            get
            {
                return this._BrokerId;
            }
            set
            {
                if (this._BrokerId == value)
                    return;
                if (this._Broker.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._BrokerId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "BrokerId", Storage = "_Broker", ThisKey = "BrokerId")]
        [BeterColumn(Internal = false, Unique = false)]
        public Broker Broker
        {
            get
            {
                return this._Broker.Entity;
            }
            set
            {
                Broker entity = this._Broker.Entity;
                if (entity == value && this._Broker.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._Broker.Entity = (Broker)null;
                    entity.Interfaces.Remove(this);
                }
                this._Broker.Entity = value;
                if (value == null)
                    throw new Exception("'Broker' is a mandatory field and cannot be set to null.");
                value.Interfaces.Add(this);
                this._BrokerId = value.BrokerId;
            }
        }

        [Column(CanBeNull = false, DbType = "varchar(150)", Storage = "_InterfaceName", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public string InterfaceName
        {
            get
            {
                return this._InterfaceName;
            }
            set
            {
                this._InterfaceName = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "varchar(500)", Storage = "_InterfaceDescription", UpdateCheck = UpdateCheck.Never)]
        public string InterfaceDescription
        {
            get
            {
                return this._InterfaceDescription;
            }
            set
            {
                this._InterfaceDescription = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = false, DbType = "int", Storage = "_ProcessBlockId", UpdateCheck = UpdateCheck.Never)]
        public int ProcessBlockId
        {
            get
            {
                return this._ProcessBlockId;
            }
            set
            {
                if (this._ProcessBlockId == value)
                    return;
                if (this._ProcessBlock.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._ProcessBlockId = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Association(IsForeignKey = true, OtherKey = "ProcessBlockId", Storage = "_ProcessBlock", ThisKey = "ProcessBlockId")]
        public ProcessBlock ProcessBlock
        {
            get
            {
                return this._ProcessBlock.Entity;
            }
            set
            {
                ProcessBlock entity = this._ProcessBlock.Entity;
                if (entity == value && this._ProcessBlock.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._ProcessBlock.Entity = (ProcessBlock)null;
                    entity.Interfaces.Remove(this);
                }
                this._ProcessBlock.Entity = value;
                if (value == null)
                    throw new Exception("'Process Block' is a mandatory field and cannot be set to null.");
                value.Interfaces.Add(this);
                this._ProcessBlockId = value.ProcessBlockId;
            }
        }

        [Column(CanBeNull = false, DbType = "int", Storage = "_ExecutionEnvironment", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public ExecutionEnvironment ExecutionEnvironment
        {
            get
            {
                return this._ExecutionEnvironment;
            }
            set
            {
                this._ExecutionEnvironment = value;
            }
        }

        [Column(CanBeNull = true, DbType = "bit", Storage = "_Running", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public bool? Running
        {
            get
            {
                return this._Running;
            }
            set
            {
                this._Running = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "int", Storage = "_ScheduleFactor", UpdateCheck = UpdateCheck.Never)]
        public int? ScheduleFactor
        {
            get
            {
                return this._ScheduleFactor;
            }
            set
            {
                this._ScheduleFactor = value;
            }
        }

        [Column(CanBeNull = true, DbType = "int", Storage = "_ScheduleFrequency", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public ScheduleCycle? ScheduleFrequency
        {
            get
            {
                return this._ScheduleFrequency;
            }
            set
            {
                this._ScheduleFrequency = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = false, DbType = "int", Storage = "_SendNotification", UpdateCheck = UpdateCheck.Never)]
        public SendNotification SendNotification
        {
            get
            {
                return this._SendNotification;
            }
            set
            {
                this._SendNotification = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "varchar(250)", Storage = "_NotificationEmail", UpdateCheck = UpdateCheck.Never)]
        public string NotificationEmail
        {
            get
            {
                return this._NotificationEmail;
            }
            set
            {
                this._NotificationEmail = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "int", Storage = "_MaximumExecutionTime", UpdateCheck = UpdateCheck.Never)]
        public int? MaximumExecutionTime
        {
            get
            {
                return this._MaximumExecutionTime;
            }
            set
            {
                this._MaximumExecutionTime = value;
            }
        }

        [Column(CanBeNull = true, DbType = "int", Storage = "_MaxBatch", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public int? MaxBatch
        {
            get
            {
                return this._MaxBatch;
            }
            set
            {
                this._MaxBatch = value;
            }
        }

        [BeterColumn(Internal = true, Unique = false)]
        [Column(CanBeNull = true, DbType = "int", Storage = "_UserId", UpdateCheck = UpdateCheck.Never)]
        public int? UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                int? nullable1 = this._UserId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._User.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._UserId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "UserId", Storage = "_User", ThisKey = "UserId")]
        [BeterColumn(Description = "User who created the entry in the table", Internal = true, Unique = false)]
        public User User
        {
            get
            {
                return this._User.Entity;
            }
            set
            {
                User entity = this._User.Entity;
                if (entity == value && this._User.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._User.Entity = (User)null;
                    entity.Interfaces.Remove(this);
                }
                this._User.Entity = value;
                if (value != null)
                {
                    value.Interfaces.Add(this);
                    this._UserId = new int?(value.UserId);
                }
                else
                    this._UserId = new int?();
            }
        }

        [BeterColumn(Internal = true, Unique = false)]
        [Column(CanBeNull = false, Storage = "_Active", UpdateCheck = UpdateCheck.Never)]
        public bool Active
        {
            get
            {
                return this._Active;
            }
        }

        [BeterColumn(Internal = true, Unique = false)]
        [Column(CanBeNull = true, Storage = "_DeActivatedOn", UpdateCheck = UpdateCheck.Never)]
        public DateTime? DeActivatedOn
        {
            get
            {
                return this._DeActivatedOn;
            }
        }

        [Association(OtherKey = "InterfaceId", Storage = "_Executions", ThisKey = "InterfaceId")]
        public EntitySet<Execution> Executions
        {
            get
            {
                return this._Executions;
            }
        }

        [Association(OtherKey = "InterfaceId", Storage = "_InterfaceSortRules", ThisKey = "InterfaceId")]
        public EntitySet<InterfaceSortRule> InterfaceSortRules
        {
            get
            {
                return this._InterfaceSortRules;
            }
        }

        [Association(OtherKey = "InterfaceId", Storage = "_InterfaceRequests", ThisKey = "InterfaceId")]
        public EntitySet<InterfaceRequest> InterfaceRequests
        {
            get
            {
                return this._InterfaceRequests;
            }
        }

        [Association(OtherKey = "InterfaceId", Storage = "_OutboundInterfaceTags", ThisKey = "InterfaceId")]
        public EntitySet<OutboundInterfaceTag> OutboundInterfaceTags
        {
            get
            {
                return this._OutboundInterfaceTags;
            }
        }

        [Association(OtherKey = "InterfaceId", Storage = "_ProcessTasks", ThisKey = "InterfaceId")]
        public EntitySet<ProcessTask> ProcessTasks
        {
            get
            {
                return this._ProcessTasks;
            }
        }

        public Interface()
        {
        }

        public Interface(DateTime TimeStamp, Broker Broker, string InterfaceName, ProcessBlock ProcessBlock, ExecutionEnvironment ExecutionEnvironment, SendNotification SendNotification)
        {
            this.TimeStamp = TimeStamp;
            this.Broker = Broker;
            this.InterfaceName = InterfaceName;
            this.ProcessBlock = ProcessBlock;
            this.ExecutionEnvironment = ExecutionEnvironment;
            this.SendNotification = SendNotification;
        }

        public void DeActivate(DateTime deActivatedOn)
        {
            this._Active = false;
            this._DeActivatedOn = new DateTime?(deActivatedOn);
        }

        public void Activate()
        {
            this._Active = true;
            this._DeActivatedOn = new DateTime?();
        }

        public override string ToString()
        {
            return this.InterfaceName.ToString();
        }

        public void ApplyInterfaceSortRules(File f)
        {
            if (!SortEngine.ConditionsPassed(f, Enumerable.ToList<ISortRule>(Enumerable.Select<InterfaceSortRule, ISortRule>(Enumerable.Where<InterfaceSortRule>((IEnumerable<InterfaceSortRule>)this.InterfaceSortRules, (Func<InterfaceSortRule, bool>)(u => u.Active)), (Func<InterfaceSortRule, ISortRule>)(u => (ISortRule)u)))))
                return;
            ProcessTask processTask = new ProcessTask(DateTime.UtcNow, this, f, false);
        }

        public DateTime LastExecuted()
        {
            if (Enumerable.Any<Execution>((IEnumerable<Execution>)this.Executions))
                return Enumerable.Max<Execution, DateTime>((IEnumerable<Execution>)this.Executions, (Func<Execution, DateTime>)(n => n.TimeStamp));
            return DateTime.MinValue;
        }
    }

    [Table]
    [BeterTable(Description = "Sends a request to the server to action a task", TrackHistory = true)]
    public class InterfaceRequest : IBase
    {
        private bool _Active = true;
        private int _InterfaceRequestId;
        private DateTime _TimeStamp;
        private int _InterfaceId;
        private EntityRef<Interface> _Interface;
        private int? _UserId;
        private EntityRef<User> _User;
        private DateTime? _DeActivatedOn;

        [Column(IsDbGenerated = true, IsPrimaryKey = true, Storage = "_InterfaceRequestId")]
        public int InterfaceRequestId
        {
            get
            {
                return this._InterfaceRequestId;
            }
            set
            {
                if (this._InterfaceRequestId == value)
                    return;
                this._InterfaceRequestId = value;
            }
        }

        [BeterColumn(Description = "When a record was inserted or last updated", Internal = true, Unique = false)]
        [Column(CanBeNull = false, DbType = "datetime", Storage = "_TimeStamp", UpdateCheck = UpdateCheck.Never)]
        public DateTime TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = false, DbType = "int", Storage = "_InterfaceId", UpdateCheck = UpdateCheck.Never)]
        public int InterfaceId
        {
            get
            {
                return this._InterfaceId;
            }
            set
            {
                if (this._InterfaceId == value)
                    return;
                if (this._Interface.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._InterfaceId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "InterfaceId", Storage = "_Interface", ThisKey = "InterfaceId")]
        [BeterColumn(Internal = false, Unique = false)]
        public Interface Interface
        {
            get
            {
                return this._Interface.Entity;
            }
            set
            {
                Interface entity = this._Interface.Entity;
                if (entity == value && this._Interface.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._Interface.Entity = (Interface)null;
                    entity.InterfaceRequests.Remove(this);
                }
                this._Interface.Entity = value;
                if (value == null)
                    throw new Exception("'Interface' is a mandatory field and cannot be set to null.");
                value.InterfaceRequests.Add(this);
                this._InterfaceId = value.InterfaceId;
            }
        }

        [Column(CanBeNull = true, DbType = "int", Storage = "_UserId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = false)]
        public int? UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                int? nullable1 = this._UserId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._User.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._UserId = value;
            }
        }

        [BeterColumn(Description = "User who created the entry in the table", Internal = true, Unique = false)]
        [Association(IsForeignKey = true, OtherKey = "UserId", Storage = "_User", ThisKey = "UserId")]
        public User User
        {
            get
            {
                return this._User.Entity;
            }
            set
            {
                User entity = this._User.Entity;
                if (entity == value && this._User.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._User.Entity = (User)null;
                    entity.InterfaceRequests.Remove(this);
                }
                this._User.Entity = value;
                if (value != null)
                {
                    value.InterfaceRequests.Add(this);
                    this._UserId = new int?(value.UserId);
                }
                else
                    this._UserId = new int?();
            }
        }

        [BeterColumn(Internal = true, Unique = false)]
        [Column(CanBeNull = false, Storage = "_Active", UpdateCheck = UpdateCheck.Never)]
        public bool Active
        {
            get
            {
                return this._Active;
            }
        }

        [BeterColumn(Internal = true, Unique = false)]
        [Column(CanBeNull = true, Storage = "_DeActivatedOn", UpdateCheck = UpdateCheck.Never)]
        public DateTime? DeActivatedOn
        {
            get
            {
                return this._DeActivatedOn;
            }
        }

        public InterfaceRequest()
        {
        }

        public InterfaceRequest(DateTime TimeStamp, Interface Interface)
        {
            this.TimeStamp = TimeStamp;
            this.Interface = Interface;
        }

        public void DeActivate(DateTime deActivatedOn)
        {
            this._Active = false;
            this._DeActivatedOn = new DateTime?(deActivatedOn);
        }

        public void Activate()
        {
            this._Active = true;
            this._DeActivatedOn = new DateTime?();
        }
    }

    [BeterTable(TrackHistory = true)]
    [Table]
    public class InterfaceSortRule : IBase, ISortRule
    {
        private bool _Active = true;
        private int _InterfaceSortRuleId;
        private DateTime _TimeStamp;
        private int _InterfaceId;
        private EntityRef<Interface> _Interface;
        private string _SortRuleName;
        private ConditionType _SortCondition;
        private FieldType _SortFieldType;
        private OperatorType _SortOperator;
        private string _SortValue;
        private string _SortValueTo;
        private FileType? _SortFileType;
        private int? _UserId;
        private EntityRef<User> _User;
        private DateTime? _DeActivatedOn;

        [Column(IsDbGenerated = true, IsPrimaryKey = true, Storage = "_InterfaceSortRuleId")]
        public int InterfaceSortRuleId
        {
            get
            {
                return this._InterfaceSortRuleId;
            }
            set
            {
                if (this._InterfaceSortRuleId == value)
                    return;
                this._InterfaceSortRuleId = value;
            }
        }

        [Column(CanBeNull = false, DbType = "datetime", Storage = "_TimeStamp", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Description = "When a record was inserted or last updated", Internal = true, Unique = false)]
        public DateTime TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
            }
        }

        [Column(CanBeNull = false, DbType = "int", Storage = "_InterfaceId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public int InterfaceId
        {
            get
            {
                return this._InterfaceId;
            }
            set
            {
                if (this._InterfaceId == value)
                    return;
                if (this._Interface.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._InterfaceId = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Association(IsForeignKey = true, OtherKey = "InterfaceId", Storage = "_Interface", ThisKey = "InterfaceId")]
        public Interface Interface
        {
            get
            {
                return this._Interface.Entity;
            }
            set
            {
                Interface entity = this._Interface.Entity;
                if (entity == value && this._Interface.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._Interface.Entity = (Interface)null;
                    entity.InterfaceSortRules.Remove(this);
                }
                this._Interface.Entity = value;
                if (value == null)
                    throw new Exception("'Interface' is a mandatory field and cannot be set to null.");
                value.InterfaceSortRules.Add(this);
                this._InterfaceId = value.InterfaceId;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "varchar(50)", Storage = "_SortRuleName", UpdateCheck = UpdateCheck.Never)]
        public string SortRuleName
        {
            get
            {
                return this._SortRuleName;
            }
            set
            {
                this._SortRuleName = value;
            }
        }

        [Column(CanBeNull = false, DbType = "int", Storage = "_SortCondition", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public ConditionType SortCondition
        {
            get
            {
                return this._SortCondition;
            }
            set
            {
                this._SortCondition = value;
            }
        }

        [Column(CanBeNull = false, DbType = "int", Storage = "_SortFieldType", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public FieldType SortFieldType
        {
            get
            {
                return this._SortFieldType;
            }
            set
            {
                this._SortFieldType = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = false, DbType = "int", Storage = "_SortOperator", UpdateCheck = UpdateCheck.Never)]
        public OperatorType SortOperator
        {
            get
            {
                return this._SortOperator;
            }
            set
            {
                this._SortOperator = value;
            }
        }

        [Column(CanBeNull = false, DbType = "varchar(50)", Storage = "_SortValue", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public string SortValue
        {
            get
            {
                return this._SortValue;
            }
            set
            {
                this._SortValue = value;
            }
        }

        [Column(CanBeNull = true, DbType = "varchar(50)", Storage = "_SortValueTo", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public string SortValueTo
        {
            get
            {
                return this._SortValueTo;
            }
            set
            {
                this._SortValueTo = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "int", Storage = "_SortFileType", UpdateCheck = UpdateCheck.Never)]
        public FileType? SortFileType
        {
            get
            {
                return this._SortFileType;
            }
            set
            {
                this._SortFileType = value;
            }
        }

        [BeterColumn(Internal = true, Unique = false)]
        [Column(CanBeNull = true, DbType = "int", Storage = "_UserId", UpdateCheck = UpdateCheck.Never)]
        public int? UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                int? nullable1 = this._UserId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._User.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._UserId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "UserId", Storage = "_User", ThisKey = "UserId")]
        [BeterColumn(Description = "User who created the entry in the table", Internal = true, Unique = false)]
        public User User
        {
            get
            {
                return this._User.Entity;
            }
            set
            {
                User entity = this._User.Entity;
                if (entity == value && this._User.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._User.Entity = (User)null;
                    entity.InterfaceSortRules.Remove(this);
                }
                this._User.Entity = value;
                if (value != null)
                {
                    value.InterfaceSortRules.Add(this);
                    this._UserId = new int?(value.UserId);
                }
                else
                    this._UserId = new int?();
            }
        }

        [BeterColumn(Internal = true, Unique = false)]
        [Column(CanBeNull = false, Storage = "_Active", UpdateCheck = UpdateCheck.Never)]
        public bool Active
        {
            get
            {
                return this._Active;
            }
        }

        [Column(CanBeNull = true, Storage = "_DeActivatedOn", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = false)]
        public DateTime? DeActivatedOn
        {
            get
            {
                return this._DeActivatedOn;
            }
        }

        public InterfaceSortRule()
        {
        }

        public InterfaceSortRule(DateTime TimeStamp, Interface Interface, ConditionType SortCondition, FieldType SortFieldType, OperatorType SortOperator, string SortValue)
        {
            this.TimeStamp = TimeStamp;
            this.Interface = Interface;
            this.SortCondition = SortCondition;
            this.SortFieldType = SortFieldType;
            this.SortOperator = SortOperator;
            this.SortValue = SortValue;
        }

        public void DeActivate(DateTime deActivatedOn)
        {
            this._Active = false;
            this._DeActivatedOn = new DateTime?(deActivatedOn);
        }

        public void Activate()
        {
            this._Active = true;
            this._DeActivatedOn = new DateTime?();
        }
    }

    public interface ISortRule
    {
        string SortValue { get; set; }

        OperatorType SortOperator { get; set; }

        DateTime TimeStamp { get; set; }

        string SortRuleName { get; set; }

        ConditionType SortCondition { get; set; }

        FieldType SortFieldType { get; set; }

        string SortValueTo { get; set; }

        User User { get; set; }

        FileType? SortFileType { get; set; }

        bool Active { get; }

        DateTime? DeActivatedOn { get; }
    }

    [Table]
    [BeterTable(TrackHistory = true)]
    public class Library : IBase
    {
        private bool _Active = true;
        private EntitySet<LibraryVersion> _LibraryVersions = new EntitySet<LibraryVersion>();
        private int _LibraryId;
        private DateTime _TimeStamp;
        private string _Name;
        private string _Description;
        private bool? _Publish;
        private int? _UserId;
        private EntityRef<User> _User;
        private DateTime? _DeActivatedOn;

        [Column(IsDbGenerated = true, IsPrimaryKey = true, Storage = "_LibraryId")]
        public int LibraryId
        {
            get
            {
                return this._LibraryId;
            }
            set
            {
                if (this._LibraryId == value)
                    return;
                this._LibraryId = value;
            }
        }

        [BeterColumn(Description = "When a record was inserted or last updated", Internal = true, Unique = false)]
        [Column(CanBeNull = false, DbType = "datetime", Storage = "_TimeStamp", UpdateCheck = UpdateCheck.Never)]
        public DateTime TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
            }
        }

        [Column(CanBeNull = true, DbType = "varchar(250)", Storage = "_Name", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this._Name = value;
            }
        }

        [Column(CanBeNull = true, DbType = "varchar(500)", Storage = "_Description", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public string Description
        {
            get
            {
                return this._Description;
            }
            set
            {
                this._Description = value;
            }
        }

        [BeterColumn(Description = "Publish Request", Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "bit", Storage = "_Publish", UpdateCheck = UpdateCheck.Never)]
        public bool? Publish
        {
            get
            {
                return this._Publish;
            }
            set
            {
                this._Publish = value;
            }
        }

        [BeterColumn(Internal = true, Unique = false)]
        [Column(CanBeNull = true, DbType = "int", Storage = "_UserId", UpdateCheck = UpdateCheck.Never)]
        public int? UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                int? nullable1 = this._UserId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._User.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._UserId = value;
            }
        }

        [BeterColumn(Description = "User who created the entry in the table", Internal = true, Unique = false)]
        [Association(IsForeignKey = true, OtherKey = "UserId", Storage = "_User", ThisKey = "UserId")]
        public User User
        {
            get
            {
                return this._User.Entity;
            }
            set
            {
                User entity = this._User.Entity;
                if (entity == value && this._User.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._User.Entity = (User)null;
                    entity.Libraries.Remove(this);
                }
                this._User.Entity = value;
                if (value != null)
                {
                    value.Libraries.Add(this);
                    this._UserId = new int?(value.UserId);
                }
                else
                    this._UserId = new int?();
            }
        }

        [BeterColumn(Internal = true, Unique = false)]
        [Column(CanBeNull = false, Storage = "_Active", UpdateCheck = UpdateCheck.Never)]
        public bool Active
        {
            get
            {
                return this._Active;
            }
        }

        [Column(CanBeNull = true, Storage = "_DeActivatedOn", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = false)]
        public DateTime? DeActivatedOn
        {
            get
            {
                return this._DeActivatedOn;
            }
        }

        [Association(OtherKey = "LibraryId", Storage = "_LibraryVersions", ThisKey = "LibraryId")]
        public EntitySet<LibraryVersion> LibraryVersions
        {
            get
            {
                return this._LibraryVersions;
            }
        }

        public Library()
        {
        }

        public Library(DateTime TimeStamp)
        {
            this.TimeStamp = TimeStamp;
        }

        public void DeActivate(DateTime deActivatedOn)
        {
            this._Active = false;
            this._DeActivatedOn = new DateTime?(deActivatedOn);
        }

        public void Activate()
        {
            this._Active = true;
            this._DeActivatedOn = new DateTime?();
        }

        public override string ToString()
        {
            return this.Name.ToString();
        }
    }

    [Table]
    [BeterTable(TrackHistory = true)]
    public class LibraryVersion : IBase
    {
        private bool _Active = true;
        private int _LibraryVersionId;
        private DateTime _TimeStamp;
        private int _LibraryId;
        private EntityRef<Library> _Library;
        private string _LibraryFilename;
        private byte[] _LibraryFile;
        private int? _UserId;
        private EntityRef<User> _User;
        private DateTime? _DeActivatedOn;

        [Column(IsDbGenerated = true, IsPrimaryKey = true, Storage = "_LibraryVersionId")]
        public int LibraryVersionId
        {
            get
            {
                return this._LibraryVersionId;
            }
            set
            {
                if (this._LibraryVersionId == value)
                    return;
                this._LibraryVersionId = value;
            }
        }

        [BeterColumn(Description = "When a record was inserted or last updated", Internal = true, Unique = false)]
        [Column(CanBeNull = false, DbType = "datetime", Storage = "_TimeStamp", UpdateCheck = UpdateCheck.Never)]
        public DateTime TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
            }
        }

        [Column(CanBeNull = false, DbType = "int", Storage = "_LibraryId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public int LibraryId
        {
            get
            {
                return this._LibraryId;
            }
            set
            {
                if (this._LibraryId == value)
                    return;
                if (this._Library.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._LibraryId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "LibraryId", Storage = "_Library", ThisKey = "LibraryId")]
        [BeterColumn(Internal = false, Unique = false)]
        public Library Library
        {
            get
            {
                return this._Library.Entity;
            }
            set
            {
                Library entity = this._Library.Entity;
                if (entity == value && this._Library.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._Library.Entity = (Library)null;
                    entity.LibraryVersions.Remove(this);
                }
                this._Library.Entity = value;
                if (value == null)
                    throw new Exception("'Library' is a mandatory field and cannot be set to null.");
                value.LibraryVersions.Add(this);
                this._LibraryId = value.LibraryId;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = false, DbType = "varchar(150)", Storage = "_LibraryFilename", UpdateCheck = UpdateCheck.Never)]
        public string LibraryFilename
        {
            get
            {
                return this._LibraryFilename;
            }
            set
            {
                this._LibraryFilename = value;
            }
        }

        [BeterColumn(Description = "Binary file content", Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "varbinary(max)", Storage = "_LibraryFile", UpdateCheck = UpdateCheck.Never)]
        public byte[] LibraryFile
        {
            get
            {
                return this._LibraryFile;
            }
            set
            {
                this._LibraryFile = value;
            }
        }

        [Column(CanBeNull = true, DbType = "int", Storage = "_UserId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = false)]
        public int? UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                int? nullable1 = this._UserId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._User.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._UserId = value;
            }
        }

        [BeterColumn(Description = "User who created the entry in the table", Internal = true, Unique = false)]
        [Association(IsForeignKey = true, OtherKey = "UserId", Storage = "_User", ThisKey = "UserId")]
        public User User
        {
            get
            {
                return this._User.Entity;
            }
            set
            {
                User entity = this._User.Entity;
                if (entity == value && this._User.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._User.Entity = (User)null;
                    entity.LibraryVersions.Remove(this);
                }
                this._User.Entity = value;
                if (value != null)
                {
                    value.LibraryVersions.Add(this);
                    this._UserId = new int?(value.UserId);
                }
                else
                    this._UserId = new int?();
            }
        }

        [Column(CanBeNull = false, Storage = "_Active", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = false)]
        public bool Active
        {
            get
            {
                return this._Active;
            }
        }

        [BeterColumn(Internal = true, Unique = false)]
        [Column(CanBeNull = true, Storage = "_DeActivatedOn", UpdateCheck = UpdateCheck.Never)]
        public DateTime? DeActivatedOn
        {
            get
            {
                return this._DeActivatedOn;
            }
        }

        public LibraryVersion()
        {
        }

        public LibraryVersion(DateTime TimeStamp, Library Library, string LibraryFilename)
        {
            this.TimeStamp = TimeStamp;
            this.Library = Library;
            this.LibraryFilename = LibraryFilename;
        }

        public void DeActivate(DateTime deActivatedOn)
        {
            this._Active = false;
            this._DeActivatedOn = new DateTime?(deActivatedOn);
        }

        public void Activate()
        {
            this._Active = true;
            this._DeActivatedOn = new DateTime?();
        }

        public override string ToString()
        {
            return this.LibraryFilename.ToString();
        }
    }

    [BeterTable(TrackHistory = true)]
    [Table]
    public class Log : IBase
    {
        private bool _Active = true;
        private int _LogId;
        private DateTime _TimeStamp;
        private BevChain.Integration.Portable.LogType? _LogType;
        private string _Message;
        private int? _UserId;
        private EntityRef<User> _User;
        private DateTime? _DeActivatedOn;

        [Column(IsDbGenerated = true, IsPrimaryKey = true, Storage = "_LogId")]
        public int LogId
        {
            get
            {
                return this._LogId;
            }
            set
            {
                if (this._LogId == value)
                    return;
                this._LogId = value;
            }
        }

        [Column(CanBeNull = false, DbType = "datetime", Storage = "_TimeStamp", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Description = "When a record was inserted or last updated", Internal = true, Unique = false)]
        public DateTime TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "int", Storage = "_LogType", UpdateCheck = UpdateCheck.Never)]
        public BevChain.Integration.Portable.LogType? LogType
        {
            get
            {
                return this._LogType;
            }
            set
            {
                this._LogType = value;
            }
        }

        [Column(CanBeNull = true, DbType = "varchar(8000)", Storage = "_Message", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public string Message
        {
            get
            {
                return this._Message;
            }
            set
            {
                this._Message = value;
            }
        }

        [BeterColumn(Internal = true, Unique = false)]
        [Column(CanBeNull = true, DbType = "int", Storage = "_UserId", UpdateCheck = UpdateCheck.Never)]
        public int? UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                int? nullable1 = this._UserId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._User.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._UserId = value;
            }
        }

        [BeterColumn(Description = "User who created the entry in the table", Internal = true, Unique = false)]
        [Association(IsForeignKey = true, OtherKey = "UserId", Storage = "_User", ThisKey = "UserId")]
        public User User
        {
            get
            {
                return this._User.Entity;
            }
            set
            {
                User entity = this._User.Entity;
                if (entity == value && this._User.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._User.Entity = (User)null;
                    entity.Logs.Remove(this);
                }
                this._User.Entity = value;
                if (value != null)
                {
                    value.Logs.Add(this);
                    this._UserId = new int?(value.UserId);
                }
                else
                    this._UserId = new int?();
            }
        }

        [Column(CanBeNull = false, Storage = "_Active", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = false)]
        public bool Active
        {
            get
            {
                return this._Active;
            }
        }

        [BeterColumn(Internal = true, Unique = false)]
        [Column(CanBeNull = true, Storage = "_DeActivatedOn", UpdateCheck = UpdateCheck.Never)]
        public DateTime? DeActivatedOn
        {
            get
            {
                return this._DeActivatedOn;
            }
        }

        public Log()
        {
        }

        public Log(DateTime TimeStamp)
        {
            this.TimeStamp = TimeStamp;
        }

        public void DeActivate(DateTime deActivatedOn)
        {
            this._Active = false;
            this._DeActivatedOn = new DateTime?(deActivatedOn);
        }

        public void Activate()
        {
            this._Active = true;
            this._DeActivatedOn = new DateTime?();
        }
    }

    public static class Messenger
    {
        public static void SendNotification(ExecutionEnvironment environment, string module, string subject, string msg)
        {
            try
            {
                Logger logger = new Logger((TimeZoneInfo)TimeZoneInfo.Utc);
                logger.LogCaption("Module: " + module.Trim());
                logger.LogCaption("Environment: " + environment.ToString());
                logger.LogBreak();
                logger.LogCaption("Notification");
                logger.LogDrawLine();
                logger.LogEntry(msg);
                EmailFunctions.SendEmailNotifications("Information Grid", "support@beter.co", "Information Grid | " + module + " | " + subject, logger);
            }
            catch (Exception ex)
            {
            }
        }

        public static void SendNotification(ExecutionEnvironment environment, string module, string subject, List<LogEvent> logs)
        {
            try
            {
                Logger logger = new Logger((TimeZoneInfo)TimeZoneInfo.Utc);
                logger.LogCaption("Module: " + module.Trim());
                logger.LogCaption("Environment: " + environment.ToString());
                logger.LogBreak();
                logger.LogCaption("Notification");
                logger.LogDrawLine();
                ((List<LogEvent>)logger.Logs).AddRange((IEnumerable<LogEvent>)logs);
                EmailFunctions.SendEmailNotifications("Information Grid", "support@beter.co", "Information Grid | " + module + " | " + subject, logger);
            }
            catch (Exception ex)
            {
            }
        }

        public static void SendNotification(string toAddress, ExecutionEnvironment environment, string module, string subject, List<LogEvent> logs)
        {
            try
            {
                Logger logger = new Logger((TimeZoneInfo)TimeZoneInfo.Utc);
                logger.LogCaption("Module: " + module.Trim());
                logger.LogCaption("Environment: " + environment.ToString());
                logger.LogBreak();
                logger.LogCaption("Notification");
                logger.LogDrawLine();
                ((List<LogEvent>)logger.Logs).AddRange((IEnumerable<LogEvent>)logs);
                EmailFunctions.SendEmailNotifications("Information Grid", toAddress, "Information Grid | " + module + " | " + subject, logger);
            }
            catch (Exception ex)
            {
            }
        }

        public static void SendNotification(ExecutionEnvironment environment, string module, Exception ex)
        {
            try
            {
                Logger logger = new Logger((TimeZoneInfo)TimeZoneInfo.Utc);
                logger.LogCaption("Module: " + module.Trim());
                logger.LogCaption("Environment: " + environment.ToString());
                logger.LogBreak();
                logger.LogCaption("Error Detail");
                logger.LogDrawLine();
                logger.LogEntry((Exception)ex);
                if (ex.InnerException != null)
                {
                    logger.LogEntry((Exception)ex.InnerException);
                    if (ex.InnerException.InnerException != null)
                        logger.LogEntry((Exception)ex.InnerException.InnerException);
                }
                EmailFunctions.SendEmailNotifications("Information Grid", "support@beter.co", "Information Grid | " + module.Trim() + " | Unknown Exception!", logger);
            }
            catch (Exception ex1)
            {
            }
        }

        public static void SendNotification(string module, Exception ex)
        {
            try
            {
                Logger logger = new Logger((TimeZoneInfo)TimeZoneInfo.Utc);
                logger.LogCaption("Module: " + module.Trim());
                logger.LogCaption("Environment: Unknown");
                logger.LogBreak();
                logger.LogCaption("Error Detail");
                logger.LogDrawLine();
                logger.LogEntry((Exception)ex);
                if (ex.InnerException != null)
                {
                    logger.LogEntry((Exception)ex.InnerException);
                    if (ex.InnerException.InnerException != null)
                        logger.LogEntry((Exception)ex.InnerException.InnerException);
                }
                EmailFunctions.SendEmailNotifications("Information Grid", "support@beter.co", "Information Grid | " + module.Trim() + " | Unknown Exception!", logger);
            }
            catch (Exception ex1)
            {
            }
        }

        public static void SendNotification(ExecutionEnvironment environment, string module, string action, Exception ex)
        {
            try
            {
                Logger logger = new Logger((TimeZoneInfo)TimeZoneInfo.Utc);
                logger.LogCaption("Module: " + module.Trim());
                logger.LogCaption("Environment: " + environment.ToString());
                logger.LogBreak();
                logger.LogCaption("Error Detail");
                logger.LogDrawLine();
                logger.LogEntry((Exception)ex);
                if (ex.InnerException != null)
                {
                    logger.LogEntry((Exception)ex.InnerException);
                    if (ex.InnerException.InnerException != null)
                        logger.LogEntry((Exception)ex.InnerException.InnerException);
                }
                EmailFunctions.SendEmailNotifications("Information Grid", "support@beter.co", "Information Grid | " + module.Trim() + " | " + action.Trim() + " Failed!", logger);
            }
            catch (Exception ex1)
            {
            }
        }

        public static void SendNotification(ExecutionEnvironment environment, string module, string action, string caption, Exception ex)
        {
            try
            {
                Logger logger = new Logger((TimeZoneInfo)TimeZoneInfo.Utc);
                logger.LogCaption("Module: " + module.Trim());
                logger.LogCaption("Environment: " + environment.ToString());
                logger.LogCaption(caption);
                logger.LogBreak();
                logger.LogCaption("Error Detail");
                logger.LogDrawLine();
                logger.LogEntry((Exception)ex);
                if (ex.InnerException != null)
                {
                    logger.LogEntry((Exception)ex.InnerException);
                    if (ex.InnerException.InnerException != null)
                        logger.LogEntry((Exception)ex.InnerException.InnerException);
                }
                EmailFunctions.SendEmailNotifications("Information Grid", "support@beter.co", "Information Grid | " + module.Trim() + " | " + action.Trim() + " Failed!", logger);
            }
            catch (Exception ex1)
            {
            }
        }
    }

    [BeterTable(TrackHistory = true)]
    [Table]
    public class OutboundInterfaceTag : IBase
    {
        private bool _Active = true;
        private int _OutboundInterfaceTagId;
        private DateTime _TimeStamp;
        private int? _InterfaceId;
        private EntityRef<Interface> _Interface;
        private int? _TagId;
        private EntityRef<Tag> _Tag;
        private int? _UserId;
        private EntityRef<User> _User;
        private DateTime? _DeActivatedOn;

        [Column(IsDbGenerated = true, IsPrimaryKey = true, Storage = "_OutboundInterfaceTagId")]
        public int OutboundInterfaceTagId
        {
            get
            {
                return this._OutboundInterfaceTagId;
            }
            set
            {
                if (this._OutboundInterfaceTagId == value)
                    return;
                this._OutboundInterfaceTagId = value;
            }
        }

        [Column(CanBeNull = false, DbType = "datetime", Storage = "_TimeStamp", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Description = "When a record was inserted or last updated", Internal = true, Unique = false)]
        public DateTime TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "int", Storage = "_InterfaceId", UpdateCheck = UpdateCheck.Never)]
        public int? InterfaceId
        {
            get
            {
                return this._InterfaceId;
            }
            set
            {
                int? nullable1 = this._InterfaceId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._Interface.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._InterfaceId = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Association(IsForeignKey = true, OtherKey = "InterfaceId", Storage = "_Interface", ThisKey = "InterfaceId")]
        public Interface Interface
        {
            get
            {
                return this._Interface.Entity;
            }
            set
            {
                Interface entity = this._Interface.Entity;
                if (entity == value && this._Interface.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._Interface.Entity = (Interface)null;
                    entity.OutboundInterfaceTags.Remove(this);
                }
                this._Interface.Entity = value;
                if (value != null)
                {
                    value.OutboundInterfaceTags.Add(this);
                    this._InterfaceId = new int?(value.InterfaceId);
                }
                else
                    this._InterfaceId = new int?();
            }
        }

        [Column(CanBeNull = true, DbType = "int", Storage = "_TagId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public int? TagId
        {
            get
            {
                return this._TagId;
            }
            set
            {
                int? nullable1 = this._TagId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._Tag.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._TagId = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Association(IsForeignKey = true, OtherKey = "TagId", Storage = "_Tag", ThisKey = "TagId")]
        public Tag Tag
        {
            get
            {
                return this._Tag.Entity;
            }
            set
            {
                Tag entity = this._Tag.Entity;
                if (entity == value && this._Tag.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._Tag.Entity = (Tag)null;
                    entity.OutboundInterfaceTags.Remove(this);
                }
                this._Tag.Entity = value;
                if (value != null)
                {
                    value.OutboundInterfaceTags.Add(this);
                    this._TagId = new int?(value.TagId);
                }
                else
                    this._TagId = new int?();
            }
        }

        [Column(CanBeNull = true, DbType = "int", Storage = "_UserId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = false)]
        public int? UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                int? nullable1 = this._UserId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._User.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._UserId = value;
            }
        }

        [BeterColumn(Description = "User who created the entry in the table", Internal = true, Unique = false)]
        [Association(IsForeignKey = true, OtherKey = "UserId", Storage = "_User", ThisKey = "UserId")]
        public User User
        {
            get
            {
                return this._User.Entity;
            }
            set
            {
                User entity = this._User.Entity;
                if (entity == value && this._User.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._User.Entity = (User)null;
                    entity.OutboundInterfaceTags.Remove(this);
                }
                this._User.Entity = value;
                if (value != null)
                {
                    value.OutboundInterfaceTags.Add(this);
                    this._UserId = new int?(value.UserId);
                }
                else
                    this._UserId = new int?();
            }
        }

        [Column(CanBeNull = false, Storage = "_Active", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = false)]
        public bool Active
        {
            get
            {
                return this._Active;
            }
        }

        [Column(CanBeNull = true, Storage = "_DeActivatedOn", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = false)]
        public DateTime? DeActivatedOn
        {
            get
            {
                return this._DeActivatedOn;
            }
        }

        public OutboundInterfaceTag()
        {
        }

        public OutboundInterfaceTag(DateTime TimeStamp)
        {
            this.TimeStamp = TimeStamp;
        }

        public void DeActivate(DateTime deActivatedOn)
        {
            this._Active = false;
            this._DeActivatedOn = new DateTime?(deActivatedOn);
        }

        public void Activate()
        {
            this._Active = true;
            this._DeActivatedOn = new DateTime?();
        }
    }

    [Table]
    [BeterTable(TrackHistory = true)]
    public class OutboundPipeSortRule : IBase, ISortRule
    {
        private bool _Active = true;
        private int _OutboundPipeSortRuleId;
        private DateTime _TimeStamp;
        private int _PipelineId;
        private EntityRef<Pipeline> _Pipeline;
        private string _SortRuleName;
        private ConditionType _SortCondition;
        private FieldType _SortFieldType;
        private OperatorType _SortOperator;
        private string _SortValue;
        private string _SortValueTo;
        private FileType? _SortFileType;
        private int? _UserId;
        private EntityRef<User> _User;
        private DateTime? _DeActivatedOn;

        [Column(IsDbGenerated = true, IsPrimaryKey = true, Storage = "_OutboundPipeSortRuleId")]
        public int OutboundPipeSortRuleId
        {
            get
            {
                return this._OutboundPipeSortRuleId;
            }
            set
            {
                if (this._OutboundPipeSortRuleId == value)
                    return;
                this._OutboundPipeSortRuleId = value;
            }
        }

        [BeterColumn(Description = "When a record was inserted or last updated", Internal = true, Unique = false)]
        [Column(CanBeNull = false, DbType = "datetime", Storage = "_TimeStamp", UpdateCheck = UpdateCheck.Never)]
        public DateTime TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = false, DbType = "int", Storage = "_PipelineId", UpdateCheck = UpdateCheck.Never)]
        public int PipelineId
        {
            get
            {
                return this._PipelineId;
            }
            set
            {
                if (this._PipelineId == value)
                    return;
                if (this._Pipeline.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._PipelineId = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Association(IsForeignKey = true, OtherKey = "PipelineId", Storage = "_Pipeline", ThisKey = "PipelineId")]
        public Pipeline Pipeline
        {
            get
            {
                return this._Pipeline.Entity;
            }
            set
            {
                Pipeline entity = this._Pipeline.Entity;
                if (entity == value && this._Pipeline.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._Pipeline.Entity = (Pipeline)null;
                    entity.OutboundPipeSortRules.Remove(this);
                }
                this._Pipeline.Entity = value;
                if (value == null)
                    throw new Exception("'Pipeline' is a mandatory field and cannot be set to null.");
                value.OutboundPipeSortRules.Add(this);
                this._PipelineId = value.PipelineId;
            }
        }

        [Column(CanBeNull = true, DbType = "varchar(50)", Storage = "_SortRuleName", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public string SortRuleName
        {
            get
            {
                return this._SortRuleName;
            }
            set
            {
                this._SortRuleName = value;
            }
        }

        [Column(CanBeNull = false, DbType = "int", Storage = "_SortCondition", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public ConditionType SortCondition
        {
            get
            {
                return this._SortCondition;
            }
            set
            {
                this._SortCondition = value;
            }
        }

        [Column(CanBeNull = false, DbType = "int", Storage = "_SortFieldType", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public FieldType SortFieldType
        {
            get
            {
                return this._SortFieldType;
            }
            set
            {
                this._SortFieldType = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = false, DbType = "int", Storage = "_SortOperator", UpdateCheck = UpdateCheck.Never)]
        public OperatorType SortOperator
        {
            get
            {
                return this._SortOperator;
            }
            set
            {
                this._SortOperator = value;
            }
        }

        [Column(CanBeNull = false, DbType = "varchar(50)", Storage = "_SortValue", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public string SortValue
        {
            get
            {
                return this._SortValue;
            }
            set
            {
                this._SortValue = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "varchar(50)", Storage = "_SortValueTo", UpdateCheck = UpdateCheck.Never)]
        public string SortValueTo
        {
            get
            {
                return this._SortValueTo;
            }
            set
            {
                this._SortValueTo = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "int", Storage = "_SortFileType", UpdateCheck = UpdateCheck.Never)]
        public FileType? SortFileType
        {
            get
            {
                return this._SortFileType;
            }
            set
            {
                this._SortFileType = value;
            }
        }

        [BeterColumn(Internal = true, Unique = false)]
        [Column(CanBeNull = true, DbType = "int", Storage = "_UserId", UpdateCheck = UpdateCheck.Never)]
        public int? UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                int? nullable1 = this._UserId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._User.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._UserId = value;
            }
        }

        [BeterColumn(Description = "User who created the entry in the table", Internal = true, Unique = false)]
        [Association(IsForeignKey = true, OtherKey = "UserId", Storage = "_User", ThisKey = "UserId")]
        public User User
        {
            get
            {
                return this._User.Entity;
            }
            set
            {
                User entity = this._User.Entity;
                if (entity == value && this._User.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._User.Entity = (User)null;
                    entity.OutboundPipeSortRules.Remove(this);
                }
                this._User.Entity = value;
                if (value != null)
                {
                    value.OutboundPipeSortRules.Add(this);
                    this._UserId = new int?(value.UserId);
                }
                else
                    this._UserId = new int?();
            }
        }

        [BeterColumn(Internal = true, Unique = false)]
        [Column(CanBeNull = false, Storage = "_Active", UpdateCheck = UpdateCheck.Never)]
        public bool Active
        {
            get
            {
                return this._Active;
            }
        }

        [Column(CanBeNull = true, Storage = "_DeActivatedOn", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = false)]
        public DateTime? DeActivatedOn
        {
            get
            {
                return this._DeActivatedOn;
            }
        }

        public OutboundPipeSortRule()
        {
        }

        public OutboundPipeSortRule(DateTime TimeStamp, Pipeline Pipeline, ConditionType SortCondition, FieldType SortFieldType, OperatorType SortOperator, string SortValue)
        {
            this.TimeStamp = TimeStamp;
            this.Pipeline = Pipeline;
            this.SortCondition = SortCondition;
            this.SortFieldType = SortFieldType;
            this.SortOperator = SortOperator;
            this.SortValue = SortValue;
        }

        public void DeActivate(DateTime deActivatedOn)
        {
            this._Active = false;
            this._DeActivatedOn = new DateTime?(deActivatedOn);
        }

        public void Activate()
        {
            this._Active = true;
            this._DeActivatedOn = new DateTime?();
        }
    }

    [BeterTable(TrackHistory = true)]
    [Table]
    public class OutboundTask : IBase
    {
        private bool _Active = true;
        private int _OutboundTaskId;
        private DateTime _TimeStamp;
        private int _PipeId;
        private EntityRef<Pipeline> _Pipe;
        private int _FileId;
        private EntityRef<File> _File;
        private bool? _Sent;
        private DateTime? _SentOn;
        private int? _UserId;
        private EntityRef<User> _User;
        private DateTime? _DeActivatedOn;

        [Column(IsDbGenerated = true, IsPrimaryKey = true, Storage = "_OutboundTaskId")]
        public int OutboundTaskId
        {
            get
            {
                return this._OutboundTaskId;
            }
            set
            {
                if (this._OutboundTaskId == value)
                    return;
                this._OutboundTaskId = value;
            }
        }

        [BeterColumn(Description = "When a record was inserted or last updated", Internal = true, Unique = false)]
        [Column(CanBeNull = false, DbType = "datetime", Storage = "_TimeStamp", UpdateCheck = UpdateCheck.Never)]
        public DateTime TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = false, DbType = "int", Storage = "_PipeId", UpdateCheck = UpdateCheck.Never)]
        public int PipeId
        {
            get
            {
                return this._PipeId;
            }
            set
            {
                if (this._PipeId == value)
                    return;
                if (this._Pipe.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._PipeId = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Association(IsForeignKey = true, OtherKey = "PipelineId", Storage = "_Pipe", ThisKey = "PipeId")]
        public Pipeline Pipe
        {
            get
            {
                return this._Pipe.Entity;
            }
            set
            {
                Pipeline entity = this._Pipe.Entity;
                if (entity == value && this._Pipe.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._Pipe.Entity = (Pipeline)null;
                    entity.PipeOutboundTasks.Remove(this);
                }
                this._Pipe.Entity = value;
                if (value == null)
                    throw new Exception("'Pipe' is a mandatory field and cannot be set to null.");
                value.PipeOutboundTasks.Add(this);
                this._PipeId = value.PipelineId;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = false, DbType = "int", Storage = "_FileId", UpdateCheck = UpdateCheck.Never)]
        public int FileId
        {
            get
            {
                return this._FileId;
            }
            set
            {
                if (this._FileId == value)
                    return;
                if (this._File.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._FileId = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Association(IsForeignKey = true, OtherKey = "FileId", Storage = "_File", ThisKey = "FileId")]
        public File File
        {
            get
            {
                return this._File.Entity;
            }
            set
            {
                File entity = this._File.Entity;
                if (entity == value && this._File.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._File.Entity = (File)null;
                    entity.OutboundTasks.Remove(this);
                }
                this._File.Entity = value;
                if (value == null)
                    throw new Exception("'File' is a mandatory field and cannot be set to null.");
                value.OutboundTasks.Add(this);
                this._FileId = value.FileId;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "bit", Storage = "_Sent", UpdateCheck = UpdateCheck.Never)]
        public bool? Sent
        {
            get
            {
                return this._Sent;
            }
            set
            {
                this._Sent = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "datetime", Storage = "_SentOn", UpdateCheck = UpdateCheck.Never)]
        public DateTime? SentOn
        {
            get
            {
                return this._SentOn;
            }
            set
            {
                this._SentOn = value;
            }
        }

        [Column(CanBeNull = true, DbType = "int", Storage = "_UserId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = false)]
        public int? UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                int? nullable1 = this._UserId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._User.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._UserId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "UserId", Storage = "_User", ThisKey = "UserId")]
        [BeterColumn(Description = "User who created the entry in the table", Internal = true, Unique = false)]
        public User User
        {
            get
            {
                return this._User.Entity;
            }
            set
            {
                User entity = this._User.Entity;
                if (entity == value && this._User.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._User.Entity = (User)null;
                    entity.OutboundTasks.Remove(this);
                }
                this._User.Entity = value;
                if (value != null)
                {
                    value.OutboundTasks.Add(this);
                    this._UserId = new int?(value.UserId);
                }
                else
                    this._UserId = new int?();
            }
        }

        [Column(CanBeNull = false, Storage = "_Active", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = false)]
        public bool Active
        {
            get
            {
                return this._Active;
            }
        }

        [Column(CanBeNull = true, Storage = "_DeActivatedOn", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = false)]
        public DateTime? DeActivatedOn
        {
            get
            {
                return this._DeActivatedOn;
            }
        }

        public OutboundTask()
        {
        }

        public OutboundTask(DateTime TimeStamp, Pipeline Pipe, File File)
        {
            this.TimeStamp = TimeStamp;
            this.Pipe = Pipe;
            this.File = File;
        }

        public bool Resend()
        {
            return true;
        }

        public void DeActivate(DateTime deActivatedOn)
        {
            this._Active = false;
            this._DeActivatedOn = new DateTime?(deActivatedOn);
        }

        public void Activate()
        {
            this._Active = true;
            this._DeActivatedOn = new DateTime?();
        }

        public override string ToString()
        {
            return this.PipeId.ToString() + " | " + this.FileId.ToString();
        }
    }

    [BeterTable(Description = "A file stream / message protocal available to transfer a file", TrackHistory = true)]
    [Table]
    public class Pipeline : IBase
    {
        private bool _Active = true;
        private EntitySet<As2Pipe> _AS2Pipes = new EntitySet<As2Pipe>();
        private EntitySet<EmailPipe> _EmailPipes = new EntitySet<EmailPipe>();
        private EntitySet<File> _Files = new EntitySet<File>();
        private EntitySet<FileTag> _FileTags = new EntitySet<FileTag>();
        private EntitySet<FtpPipe> _FTPPipes = new EntitySet<FtpPipe>();
        private EntitySet<HttpPipe> _HttpPipes = new EntitySet<HttpPipe>();
        private EntitySet<ConnectionPipe> _ConnectionPipes = new EntitySet<ConnectionPipe>();
        private EntitySet<InboundPipeTag> _InboundPipeTags = new EntitySet<InboundPipeTag>();
        private EntitySet<OutboundPipeSortRule> _OutboundPipeSortRules = new EntitySet<OutboundPipeSortRule>();
        private EntitySet<OutboundTask> _PipeOutboundTasks = new EntitySet<OutboundTask>();
        private int _PipelineId;
        private DateTime _TimeStamp;
        private int _ConnectionId;
        private EntityRef<Connection> _Connection;
        private string _PipelineName;
        private string _PipelineDescription;
        private int? _UserId;
        private EntityRef<User> _User;
        private DateTime? _DeActivatedOn;

        [Column(IsDbGenerated = true, IsPrimaryKey = true, Storage = "_PipelineId")]
        public int PipelineId
        {
            get
            {
                return this._PipelineId;
            }
            set
            {
                if (this._PipelineId == value)
                    return;
                this._PipelineId = value;
            }
        }

        [BeterColumn(Description = "When a record was inserted or last updated", Internal = true, Unique = false)]
        [Column(CanBeNull = false, DbType = "datetime", Storage = "_TimeStamp", UpdateCheck = UpdateCheck.Never)]
        public DateTime TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
            }
        }

        [Column(CanBeNull = false, DbType = "int", Storage = "_ConnectionId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = true)]
        public int ConnectionId
        {
            get
            {
                return this._ConnectionId;
            }
            set
            {
                if (this._ConnectionId == value)
                    return;
                if (this._Connection.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._ConnectionId = value;
            }
        }

        [BeterColumn(Internal = false, Unique = true)]
        [Association(IsForeignKey = true, OtherKey = "ConnectionId", Storage = "_Connection", ThisKey = "ConnectionId")]
        public Connection Connection
        {
            get
            {
                return this._Connection.Entity;
            }
            set
            {
                Connection entity = this._Connection.Entity;
                if (entity == value && this._Connection.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._Connection.Entity = (Connection)null;
                    entity.Pipelines.Remove(this);
                }
                this._Connection.Entity = value;
                if (value == null)
                    throw new Exception("'Connection' is a mandatory field and cannot be set to null.");
                value.Pipelines.Add(this);
                this._ConnectionId = value.ConnectionId;
            }
        }

        [BeterColumn(Internal = false, Unique = true)]
        [Column(CanBeNull = false, DbType = "varchar(150)", Storage = "_PipelineName", UpdateCheck = UpdateCheck.Never)]
        public string PipelineName
        {
            get
            {
                return this._PipelineName;
            }
            set
            {
                this._PipelineName = value;
            }
        }

        [Column(CanBeNull = true, DbType = "varchar(250)", Storage = "_PipelineDescription", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public string PipelineDescription
        {
            get
            {
                return this._PipelineDescription;
            }
            set
            {
                this._PipelineDescription = value;
            }
        }

        [BeterColumn(Internal = true, Unique = false)]
        [Column(CanBeNull = true, DbType = "int", Storage = "_UserId", UpdateCheck = UpdateCheck.Never)]
        public int? UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                int? nullable1 = this._UserId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._User.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._UserId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "UserId", Storage = "_User", ThisKey = "UserId")]
        [BeterColumn(Description = "User who created the entry in the table", Internal = true, Unique = false)]
        public User User
        {
            get
            {
                return this._User.Entity;
            }
            set
            {
                User entity = this._User.Entity;
                if (entity == value && this._User.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._User.Entity = (User)null;
                    entity.Pipelines.Remove(this);
                }
                this._User.Entity = value;
                if (value != null)
                {
                    value.Pipelines.Add(this);
                    this._UserId = new int?(value.UserId);
                }
                else
                    this._UserId = new int?();
            }
        }

        [Column(CanBeNull = false, Storage = "_Active", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = true, UpdateUnique = true)]
        public bool Active
        {
            get
            {
                return this._Active;
            }
        }

        [Column(CanBeNull = true, Storage = "_DeActivatedOn", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(IgnoreUniqueUpdateCheck = true, Internal = true, Unique = true, UpdateUnique = true)]
        public DateTime? DeActivatedOn
        {
            get
            {
                return this._DeActivatedOn;
            }
        }

        [Association(OtherKey = "PipelineId", Storage = "_AS2Pipes", ThisKey = "PipelineId")]
        public EntitySet<As2Pipe> AS2Pipes
        {
            get
            {
                return this._AS2Pipes;
            }
        }

        [Association(OtherKey = "PipelineId", Storage = "_EmailPipes", ThisKey = "PipelineId")]
        public EntitySet<EmailPipe> EmailPipes
        {
            get
            {
                return this._EmailPipes;
            }
        }

        [Association(OtherKey = "PipelineId", Storage = "_Files", ThisKey = "PipelineId")]
        public EntitySet<File> Files
        {
            get
            {
                return this._Files;
            }
        }

        [Association(OtherKey = "PipelineId", Storage = "_FileTags", ThisKey = "PipelineId")]
        public EntitySet<FileTag> FileTags
        {
            get
            {
                return this._FileTags;
            }
        }

        [Association(OtherKey = "PipelineId", Storage = "_FTPPipes", ThisKey = "PipelineId")]
        public EntitySet<FtpPipe> FTPPipes
        {
            get
            {
                return this._FTPPipes;
            }
        }

        [Association(OtherKey = "PipelineId", Storage = "_ConnectionPipes", ThisKey = "PipelineId")]
        public EntitySet<ConnectionPipe> ConnectionPipe
        {
            get
            {
                return this._ConnectionPipes;
            }
        }

        [Association(OtherKey = "PipelineId", Storage = "_HttpPipes", ThisKey = "PipelineId")]
        public EntitySet<HttpPipe> HttpPipes
        {
            get
            {
                return this._HttpPipes;
            }
        }

        [Association(OtherKey = "PipelineId", Storage = "_InboundPipeTags", ThisKey = "PipelineId")]
        public EntitySet<InboundPipeTag> InboundPipeTags
        {
            get
            {
                return this._InboundPipeTags;
            }
        }

        [Association(OtherKey = "PipelineId", Storage = "_OutboundPipeSortRules", ThisKey = "PipelineId")]
        public EntitySet<OutboundPipeSortRule> OutboundPipeSortRules
        {
            get
            {
                return this._OutboundPipeSortRules;
            }
        }

        [Association(OtherKey = "PipeId", Storage = "_PipeOutboundTasks", ThisKey = "PipelineId")]
        public EntitySet<OutboundTask> PipeOutboundTasks
        {
            get
            {
                return this._PipeOutboundTasks;
            }
        }

        public Pipeline()
        {
        }

        public Pipeline(DateTime TimeStamp, Connection Connection, string PipelineName)
        {
            this.TimeStamp = TimeStamp;
            this.Connection = Connection;
            this.PipelineName = PipelineName;
        }

        public void DeActivate(DateTime deActivatedOn)
        {
            this._Active = false;
            this._DeActivatedOn = new DateTime?(deActivatedOn);
        }

        public void Activate()
        {
            this._Active = true;
            this._DeActivatedOn = new DateTime?();
        }

        public override string ToString()
        {
            return this.PipelineName.ToString();
        }

        public File AddFile(byte[] content, File sourceFile = null)
        {
            return this.AddFile("", content, sourceFile);
        }

        public File AddFile(object content, File sourceFile = null)
        {
            return (File)this.AddFile("", content, sourceFile);
        }

        public File AddFile(string filename, byte[] content, File sourceFile = null)
        {
            File f = new File(filename, content, sourceFile);
            this.ApplyInboundSortRules(f, true);
            return f;
        }

        public File AddFile(string filename, object content, File sourceFile = null)
        {
            File file = new File(filename, content, sourceFile);
            this.ApplyInboundSortRules(file, true);
            return file;
        }

        public File AddFile(string filename, object content, Directory directory, File sourceFile = null)
        {
            File file = new File(filename, content, sourceFile);
            FileDirectory fileDirectory = new FileDirectory(DateTime.UtcNow, file, directory);
            List<Tag> tags = directory.GetTags();
            FileTag fileTag = file.AddTag(tags.First<Tag>(), tags.First<Tag>() == tags.Last<Tag>(), this, null);
            int num = 1;
            foreach (Tag tag in tags.Skip<Tag>(1))
            {
                num++;
                FileTag fileTag1 = fileTag.AddTag(tag, tag == tags.Last<Tag>(), this, null);
                fileTag = fileTag1;
            }
            this.ApplyInboundSortRules(file, false);
            return file;
        }

        public void ApplyInboundSortRules(File f, bool addDirectory = true)
        {
            f.Pipeline = this;
            foreach (InboundPipeTag inboundPipeTag in Enumerable.Where<InboundPipeTag>((IEnumerable<InboundPipeTag>)this.InboundPipeTags, (Func<InboundPipeTag, bool>)(n => n.Active)))
            {
                if (SortEngine.ConditionsPassed(f, Enumerable.ToList<ISortRule>(Enumerable.Select<InboundPipeTagSortRule, ISortRule>(Enumerable.Where<InboundPipeTagSortRule>((IEnumerable<InboundPipeTagSortRule>)inboundPipeTag.InboundPipeTagSortRules, (Func<InboundPipeTagSortRule, bool>)(u => u.Active)), (Func<InboundPipeTagSortRule, ISortRule>)(u => (ISortRule)u)))))
                    f.AddTag(inboundPipeTag.Tag, false, this, (Execution)null);
            }
            f.ApplySortRules(addDirectory);
        }

        public void ApplyOutboundSortRules(File f)
        {
            if (!SortEngine.ConditionsPassed(f, Enumerable.ToList<ISortRule>(Enumerable.Select<OutboundPipeSortRule, ISortRule>(Enumerable.Where<OutboundPipeSortRule>((IEnumerable<OutboundPipeSortRule>)this.OutboundPipeSortRules, (Func<OutboundPipeSortRule, bool>)(u => u.Active)), (Func<OutboundPipeSortRule, ISortRule>)(u => (ISortRule)u)))))
                return;
            OutboundTask outboundTask = new OutboundTask(DateTime.UtcNow, this, f);
        }

        public IOrderedEnumerable<OutboundTask> GetOutboundTasks()
        {
            return Enumerable.OrderByDescending<OutboundTask, DateTime>(Enumerable.Where<OutboundTask>((IEnumerable<OutboundTask>)this.PipeOutboundTasks, (Func<OutboundTask, bool>)(u =>
            {
                if (u.Active)
                    return !u.Sent.GetValueOrDefault();
                return false;
            })), (Func<OutboundTask, DateTime>)(u => u.TimeStamp));
        }
    }

    [BeterTable(Description = "Transform & generate process - import, export or transform file route", TrackHistory = true)]
    [Table]
    public class ProcessBlock : IBase
    {
        private bool _Active = true;
        private EntitySet<Interface> _Interfaces = new EntitySet<Interface>();
        private int _ProcessBlockId;
        private DateTime _TimeStamp;
        private int _BrokerId;
        private EntityRef<Broker> _Broker;
        private string _ProcessName;
        private string _DynamicCode;
        private string _ProcessDescription;
        private string _EmailNotification;
        private int? _UserId;
        private EntityRef<User> _User;
        private DateTime? _DeActivatedOn;

        [Column(IsDbGenerated = true, IsPrimaryKey = true, Storage = "_ProcessBlockId")]
        public int ProcessBlockId
        {
            get
            {
                return this._ProcessBlockId;
            }
            set
            {
                if (this._ProcessBlockId == value)
                    return;
                this._ProcessBlockId = value;
            }
        }

        [Column(CanBeNull = false, DbType = "datetime", Storage = "_TimeStamp", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Description = "When a record was inserted or last updated", Internal = true, Unique = false)]
        public DateTime TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = false, DbType = "int", Storage = "_BrokerId", UpdateCheck = UpdateCheck.Never)]
        public int BrokerId
        {
            get
            {
                return this._BrokerId;
            }
            set
            {
                if (this._BrokerId == value)
                    return;
                if (this._Broker.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._BrokerId = value;
            }
        }

        [BeterColumn(Description = "Owner of Process Bock", Internal = false, Unique = false)]
        [Association(IsForeignKey = true, OtherKey = "BrokerId", Storage = "_Broker", ThisKey = "BrokerId")]
        public Broker Broker
        {
            get
            {
                return this._Broker.Entity;
            }
            set
            {
                Broker entity = this._Broker.Entity;
                if (entity == value && this._Broker.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._Broker.Entity = (Broker)null;
                    entity.ProcessBlocks.Remove(this);
                }
                this._Broker.Entity = value;
                if (value == null)
                    throw new Exception("'Broker' is a mandatory field and cannot be set to null.");
                value.ProcessBlocks.Add(this);
                this._BrokerId = value.BrokerId;
            }
        }

        [BeterColumn(Description = "Name of Process Block", Internal = false, Unique = true)]
        [Column(CanBeNull = false, DbType = "varchar(50)", Storage = "_ProcessName", UpdateCheck = UpdateCheck.Never)]
        public string ProcessName
        {
            get
            {
                return this._ProcessName;
            }
            set
            {
                this._ProcessName = value;
            }
        }

        [BeterColumn(Description = "Implementation Code of Broker", Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "varchar(max)", Storage = "_DynamicCode", UpdateCheck = UpdateCheck.Never)]
        public string DynamicCode
        {
            get
            {
                return this._DynamicCode;
            }
            set
            {
                this._DynamicCode = value;
            }
        }

        [Column(CanBeNull = true, DbType = "varchar(500)", Storage = "_ProcessDescription", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Description = "Description of Process Block", Internal = false, Unique = false)]
        public string ProcessDescription
        {
            get
            {
                return this._ProcessDescription;
            }
            set
            {
                this._ProcessDescription = value;
            }
        }

        [Column(CanBeNull = true, DbType = "varchar(250)", Storage = "_EmailNotification", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public string EmailNotification
        {
            get
            {
                return this._EmailNotification;
            }
            set
            {
                this._EmailNotification = value;
            }
        }

        [Column(CanBeNull = true, DbType = "int", Storage = "_UserId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = false)]
        public int? UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                int? nullable1 = this._UserId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._User.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._UserId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "UserId", Storage = "_User", ThisKey = "UserId")]
        [BeterColumn(Description = "User who created the entry in the table", Internal = true, Unique = false)]
        public User User
        {
            get
            {
                return this._User.Entity;
            }
            set
            {
                User entity = this._User.Entity;
                if (entity == value && this._User.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._User.Entity = (User)null;
                    entity.ProcessBlocks.Remove(this);
                }
                this._User.Entity = value;
                if (value != null)
                {
                    value.ProcessBlocks.Add(this);
                    this._UserId = new int?(value.UserId);
                }
                else
                    this._UserId = new int?();
            }
        }

        [Column(CanBeNull = false, Storage = "_Active", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = true, UpdateUnique = true)]
        public bool Active
        {
            get
            {
                return this._Active;
            }
        }

        [Column(CanBeNull = true, Storage = "_DeActivatedOn", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(IgnoreUniqueUpdateCheck = true, Internal = true, Unique = true, UpdateUnique = true)]
        public DateTime? DeActivatedOn
        {
            get
            {
                return this._DeActivatedOn;
            }
        }

        [Association(OtherKey = "ProcessBlockId", Storage = "_Interfaces", ThisKey = "ProcessBlockId")]
        public EntitySet<Interface> Interfaces
        {
            get
            {
                return this._Interfaces;
            }
        }

        public bool InSync { get; set; }

        public bool IsOpen { get; set; }

        public bool IsSaved { get; set; }

        public bool ExistsLocally { get; set; }

        public string DisplayName { get; set; }

        public bool ActiveOne { get; set; }

        public ProcessBlock()
        {
        }

        public ProcessBlock(DateTime TimeStamp, Broker Broker, string ProcessName)
        {
            this.TimeStamp = TimeStamp;
            this.Broker = Broker;
            this.ProcessName = ProcessName;
        }

        public void DeActivate(DateTime deActivatedOn)
        {
            this._Active = false;
            this._DeActivatedOn = new DateTime?(deActivatedOn);
        }

        public void Activate()
        {
            this._Active = true;
            this._DeActivatedOn = new DateTime?();
        }

        public override string ToString()
        {
            return this.BrokerId.ToString() + " | " + this.ProcessName.ToString();
        }
    }

    [Table]
    [BeterTable(TrackHistory = true)]
    public class ProcessTask : IBase
    {
        private bool _Active = true;
        private int _ProcessTaskId;
        private DateTime _TimeStamp;
        private int _InterfaceId;
        private EntityRef<Interface> _Interface;
        private int _FileId;
        private EntityRef<File> _File;
        private bool _Processed;
        private DateTime? _ProcessedOn;
        private int? _UserId;
        private EntityRef<User> _User;
        private DateTime? _DeActivatedOn;

        [Column(IsDbGenerated = true, IsPrimaryKey = true, Storage = "_ProcessTaskId")]
        public int ProcessTaskId
        {
            get
            {
                return this._ProcessTaskId;
            }
            set
            {
                if (this._ProcessTaskId == value)
                    return;
                this._ProcessTaskId = value;
            }
        }

        [BeterColumn(Description = "When a record was inserted or last updated", Internal = true, Unique = false)]
        [Column(CanBeNull = false, DbType = "datetime", Storage = "_TimeStamp", UpdateCheck = UpdateCheck.Never)]
        public DateTime TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = false, DbType = "int", Storage = "_InterfaceId", UpdateCheck = UpdateCheck.Never)]
        public int InterfaceId
        {
            get
            {
                return this._InterfaceId;
            }
            set
            {
                if (this._InterfaceId == value)
                    return;
                if (this._Interface.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._InterfaceId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "InterfaceId", Storage = "_Interface", ThisKey = "InterfaceId")]
        [BeterColumn(Internal = false, Unique = false)]
        public Interface Interface
        {
            get
            {
                return this._Interface.Entity;
            }
            set
            {
                Interface entity = this._Interface.Entity;
                if (entity == value && this._Interface.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._Interface.Entity = (Interface)null;
                    entity.ProcessTasks.Remove(this);
                }
                this._Interface.Entity = value;
                if (value == null)
                    throw new Exception("'Interface' is a mandatory field and cannot be set to null.");
                value.ProcessTasks.Add(this);
                this._InterfaceId = value.InterfaceId;
            }
        }

        [Column(CanBeNull = false, DbType = "int", Storage = "_FileId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public int FileId
        {
            get
            {
                return this._FileId;
            }
            set
            {
                if (this._FileId == value)
                    return;
                if (this._File.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._FileId = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Association(IsForeignKey = true, OtherKey = "FileId", Storage = "_File", ThisKey = "FileId")]
        public File File
        {
            get
            {
                return this._File.Entity;
            }
            set
            {
                File entity = this._File.Entity;
                if (entity == value && this._File.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._File.Entity = (File)null;
                    entity.ProcessTasks.Remove(this);
                }
                this._File.Entity = value;
                if (value == null)
                    throw new Exception("'File' is a mandatory field and cannot be set to null.");
                value.ProcessTasks.Add(this);
                this._FileId = value.FileId;
            }
        }

        [Column(CanBeNull = false, DbType = "bit", Storage = "_Processed", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public bool Processed
        {
            get
            {
                return this._Processed;
            }
            set
            {
                this._Processed = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "datetime", Storage = "_ProcessedOn", UpdateCheck = UpdateCheck.Never)]
        public DateTime? ProcessedOn
        {
            get
            {
                return this._ProcessedOn;
            }
            set
            {
                this._ProcessedOn = value;
            }
        }

        [Column(CanBeNull = true, DbType = "int", Storage = "_UserId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = false)]
        public int? UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                int? nullable1 = this._UserId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._User.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._UserId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "UserId", Storage = "_User", ThisKey = "UserId")]
        [BeterColumn(Description = "User who created the entry in the table", Internal = true, Unique = false)]
        public User User
        {
            get
            {
                return this._User.Entity;
            }
            set
            {
                User entity = this._User.Entity;
                if (entity == value && this._User.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._User.Entity = (User)null;
                    entity.ProcessTasks.Remove(this);
                }
                this._User.Entity = value;
                if (value != null)
                {
                    value.ProcessTasks.Add(this);
                    this._UserId = new int?(value.UserId);
                }
                else
                    this._UserId = new int?();
            }
        }

        [BeterColumn(Internal = true, Unique = false)]
        [Column(CanBeNull = false, Storage = "_Active", UpdateCheck = UpdateCheck.Never)]
        public bool Active
        {
            get
            {
                return this._Active;
            }
        }

        [BeterColumn(Internal = true, Unique = false)]
        [Column(CanBeNull = true, Storage = "_DeActivatedOn", UpdateCheck = UpdateCheck.Never)]
        public DateTime? DeActivatedOn
        {
            get
            {
                return this._DeActivatedOn;
            }
        }

        public ProcessTask()
        {
        }

        public ProcessTask(DateTime TimeStamp, Interface Interface, File File, bool Processed)
        {
            this.TimeStamp = TimeStamp;
            this.Interface = Interface;
            this.File = File;
            this.Processed = Processed;
        }

        public void DeActivate(DateTime deActivatedOn)
        {
            this._Active = false;
            this._DeActivatedOn = new DateTime?(deActivatedOn);
        }

        public void Activate()
        {
            this._Active = true;
            this._DeActivatedOn = new DateTime?();
        }

        public override string ToString()
        {
            return this.InterfaceId.ToString() + " | " + this.FileId.ToString();
        }
    }

    public abstract class ScheduledExecution : Execution
    {
        public ScheduledExecution(BETERIntegrationLink db)
        {
        }

        public abstract int ExecuteProccess();

        protected void ExportFile(string filename, object content)
        {
            (new File(filename, content)).Execution = this;
        }

        protected void ExportFile(string filename, object content, XTag tag)
        {
            (new File(filename, content)).Execution = this;
        }

        protected void ExportFile(string filename, object content, params XTag[] tags)
        {
            (new File(filename, content)).Execution = this;
        }

        protected void ExportFile(string filename, object content, List<XTag> tags)
        {
            (new File(filename, content)).Execution = this;
        }
    }

    [Table]
    [BeterTable(Description = "File pool refers to queue when message is awaiting processing or folder when file is being stored", TrackHistory = true)]
    public class SecurityPool : IBase
    {
        private bool _Active = true;
        private int _SecurityPoolId;
        private DateTime _TimeStamp;
        private int _BrokerId;
        private EntityRef<Broker> _Broker;
        private string _PoolName;
        private string _PoolDescription;
        private int? _UserId;
        private EntityRef<User> _User;
        private DateTime? _DeActivatedOn;

        [Column(IsDbGenerated = true, IsPrimaryKey = true, Storage = "_SecurityPoolId")]
        public int SecurityPoolId
        {
            get
            {
                return this._SecurityPoolId;
            }
            set
            {
                if (this._SecurityPoolId == value)
                    return;
                this._SecurityPoolId = value;
            }
        }

        [Column(CanBeNull = false, DbType = "datetime", Storage = "_TimeStamp", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Description = "When a record was inserted or last updated", Internal = true, Unique = false)]
        public DateTime TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
            }
        }

        [BeterColumn(Internal = false, Unique = true)]
        [Column(CanBeNull = false, DbType = "int", Storage = "_BrokerId", UpdateCheck = UpdateCheck.Never)]
        public int BrokerId
        {
            get
            {
                return this._BrokerId;
            }
            set
            {
                if (this._BrokerId == value)
                    return;
                if (this._Broker.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._BrokerId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "BrokerId", Storage = "_Broker", ThisKey = "BrokerId")]
        [BeterColumn(Description = "Broker beloning to the pool", Internal = false, Unique = true)]
        public Broker Broker
        {
            get
            {
                return this._Broker.Entity;
            }
            set
            {
                Broker entity = this._Broker.Entity;
                if (entity == value && this._Broker.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._Broker.Entity = (Broker)null;
                    entity.SecurityPools.Remove(this);
                }
                this._Broker.Entity = value;
                if (value == null)
                    throw new Exception("'Broker' is a mandatory field and cannot be set to null.");
                value.SecurityPools.Add(this);
                this._BrokerId = value.BrokerId;
            }
        }

        [Column(CanBeNull = false, DbType = "varchar(50)", Storage = "_PoolName", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = true)]
        public string PoolName
        {
            get
            {
                return this._PoolName;
            }
            set
            {
                this._PoolName = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "varchar(250)", Storage = "_PoolDescription", UpdateCheck = UpdateCheck.Never)]
        public string PoolDescription
        {
            get
            {
                return this._PoolDescription;
            }
            set
            {
                this._PoolDescription = value;
            }
        }

        [BeterColumn(Internal = true, Unique = false)]
        [Column(CanBeNull = true, DbType = "int", Storage = "_UserId", UpdateCheck = UpdateCheck.Never)]
        public int? UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                int? nullable1 = this._UserId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._User.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._UserId = value;
            }
        }

        [BeterColumn(Description = "User who created the entry in the table", Internal = true, Unique = false)]
        [Association(IsForeignKey = true, OtherKey = "UserId", Storage = "_User", ThisKey = "UserId")]
        public User User
        {
            get
            {
                return this._User.Entity;
            }
            set
            {
                User entity = this._User.Entity;
                if (entity == value && this._User.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._User.Entity = (User)null;
                    entity.SecurityPools.Remove(this);
                }
                this._User.Entity = value;
                if (value != null)
                {
                    value.SecurityPools.Add(this);
                    this._UserId = new int?(value.UserId);
                }
                else
                    this._UserId = new int?();
            }
        }

        [BeterColumn(Internal = true, Unique = true, UpdateUnique = true)]
        [Column(CanBeNull = false, Storage = "_Active", UpdateCheck = UpdateCheck.Never)]
        public bool Active
        {
            get
            {
                return this._Active;
            }
        }

        [Column(CanBeNull = true, Storage = "_DeActivatedOn", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(IgnoreUniqueUpdateCheck = true, Internal = true, Unique = true, UpdateUnique = true)]
        public DateTime? DeActivatedOn
        {
            get
            {
                return this._DeActivatedOn;
            }
        }

        public SecurityPool()
        {
        }

        public SecurityPool(DateTime TimeStamp, Broker Broker, string PoolName)
        {
            this.TimeStamp = TimeStamp;
            this.Broker = Broker;
            this.PoolName = PoolName;
        }

        public void DeActivate(DateTime deActivatedOn)
        {
            this._Active = false;
            this._DeActivatedOn = new DateTime?(deActivatedOn);
        }

        public void Activate()
        {
            this._Active = true;
            this._DeActivatedOn = new DateTime?();
        }
    }

    public static class SortEngine
    {
        public static bool ConditionsPassed(File f, List<ISortRule> rules)
        {
            if (rules != null)
            {
                foreach (ISortRule rule in rules)
                {
                    switch (rule.SortCondition)
                    {
                        case ConditionType.And:
                            if (!SortEngine.FieldTypePassed(f, rule))
                                return false;
                            continue;
                        case ConditionType.AndNot:
                            if (SortEngine.FieldTypePassed(f, rule))
                                return false;
                            continue;
                        default:
                            throw new NotImplementedException(rule.SortCondition.ToString());
                    }
                }
            }
            return true;
        }

        private static bool FieldTypePassed(File f, ISortRule rule)
        {
            switch (rule.SortFieldType)
            {
                case FieldType.Filename:
                    return SortEngine.OperatorPassed(f.Filename, rule);
                case FieldType.Tag:
                    if (rule.SortOperator == OperatorType.Equals || rule.SortOperator == OperatorType.Contains || (rule.SortOperator == OperatorType.BeginsWith || rule.SortOperator == OperatorType.EndsWith))
                    {
                        foreach (FileTag fileTag in Enumerable.Where<FileTag>((IEnumerable<FileTag>)f.FileTags, (Func<FileTag, bool>)(n => n.Active)))
                        {
                            if (SortEngine.OperatorPassed(fileTag.Tag.TagName, rule))
                                return true;
                        }
                        return false;
                    }
                    foreach (FileTag fileTag in Enumerable.Where<FileTag>((IEnumerable<FileTag>)f.FileTags, (Func<FileTag, bool>)(n => n.Active)))
                    {
                        if (!SortEngine.OperatorPassed(fileTag.Tag.TagName, rule))
                            return false;
                    }
                    return true;
                case FieldType.SourceFilename:
                    if (f.SourceFile != null)
                        return SortEngine.OperatorPassed(f.SourceFile.Filename, rule);
                    return SortEngine.OperatorPassed(string.Empty, rule);
                case FieldType.FileType:
                    return SortEngine.OperatorPassed(f.FileType, rule);
                case FieldType.From:
                    return SortEngine.OperatorPassed(f.GetFrom(), rule);
                default:
                    throw new NotImplementedException(rule.SortOperator.ToString());
            }
        }

        private static bool OperatorPassed(string value, ISortRule rule)
        {
            value = (value ?? string.Empty).ToLower();
            string str = rule.SortValue.ToLower();
            switch (rule.SortOperator)
            {
                case OperatorType.Equals:
                    return value == str;
                case OperatorType.NotEquals:
                    return value != str;
                case OperatorType.Contains:
                    return value.Contains(str);
                case OperatorType.DoesNotContain:
                    return !value.Contains(str);
                case OperatorType.BeginsWith:
                    return value.StartsWith(str);
                case OperatorType.EndsWith:
                    return value.EndsWith(str);
                case OperatorType.DoesNotBeginWith:
                    return !value.StartsWith(str);
                case OperatorType.DoesNotEndWith:
                    return !value.EndsWith(str);
                default:
                    throw new NotImplementedException(rule.SortOperator.ToString());
            }
        }

        private static bool OperatorPassed(FileType value, ISortRule rule)
        {
            switch (rule.SortOperator)
            {
                case OperatorType.Equals:
                    FileType fileType1 = value;
                    FileType? sortFileType1 = rule.SortFileType;
                    if (fileType1 == sortFileType1.GetValueOrDefault())
                        return sortFileType1.HasValue;
                    return false;
                case OperatorType.NotEquals:
                    FileType fileType2 = value;
                    FileType? sortFileType2 = rule.SortFileType;
                    if (fileType2 == sortFileType2.GetValueOrDefault())
                        return !sortFileType2.HasValue;
                    return true;
                default:
                    throw new NotImplementedException(rule.SortOperator.ToString());
            }
        }
    }

    [BeterTable(TrackHistory = true)]
    [Table]
    public class Tag : IBase
    {
        private bool _Active = true;
        private EntitySet<BrokerTag> _BrokerTags = new EntitySet<BrokerTag>();
        private EntitySet<Directory> _Directories = new EntitySet<Directory>();
        private EntitySet<FileTag> _FileTags = new EntitySet<FileTag>();
        private EntitySet<InboundPipeTag> _InboundPipeTags = new EntitySet<InboundPipeTag>();
        private EntitySet<OutboundInterfaceTag> _OutboundInterfaceTags = new EntitySet<OutboundInterfaceTag>();
        private EntitySet<Tag> _ParentTags = new EntitySet<Tag>();
        private int _TagId;
        private DateTime _TimeStamp;
        private int? _BrokerId;
        private EntityRef<Broker> _Broker;
        private int? _ParentTagId;
        private EntityRef<Tag> _ParentTag;
        private string _TagName;
        private string _TagDescription;
        private bool? _ExcludeFromFileDirectory;
        private int? _UserId;
        private EntityRef<User> _User;
        private DateTime? _DeActivatedOn;

        [Column(IsDbGenerated = true, IsPrimaryKey = true, Storage = "_TagId")]
        public int TagId
        {
            get
            {
                return this._TagId;
            }
            set
            {
                if (this._TagId == value)
                    return;
                this._TagId = value;
            }
        }

        [BeterColumn(Description = "When a record was inserted or last updated", Internal = true, Unique = false)]
        [Column(CanBeNull = false, DbType = "datetime", Storage = "_TimeStamp", UpdateCheck = UpdateCheck.Never)]
        public DateTime TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
            }
        }

        [Column(CanBeNull = true, DbType = "int", Storage = "_BrokerId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = true)]
        public int? BrokerId
        {
            get
            {
                return this._BrokerId;
            }
            set
            {
                int? nullable1 = this._BrokerId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._Broker.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._BrokerId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "BrokerId", Storage = "_Broker", ThisKey = "BrokerId")]
        [BeterColumn(Description = "Owner", Internal = false, Unique = true)]
        public Broker Broker
        {
            get
            {
                return this._Broker.Entity;
            }
            set
            {
                Broker entity = this._Broker.Entity;
                if (entity == value && this._Broker.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._Broker.Entity = (Broker)null;
                    entity.Tags.Remove(this);
                }
                this._Broker.Entity = value;
                if (value != null)
                {
                    value.Tags.Add(this);
                    this._BrokerId = new int?(value.BrokerId);
                }
                else
                    this._BrokerId = new int?();
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "int", Storage = "_ParentTagId", UpdateCheck = UpdateCheck.Never)]
        public int? ParentTagId
        {
            get
            {
                return this._ParentTagId;
            }
            set
            {
                int? nullable1 = this._ParentTagId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._ParentTag.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._ParentTagId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "TagId", Storage = "_ParentTag", ThisKey = "ParentTagId")]
        [BeterColumn(Internal = false, Unique = false)]
        public Tag ParentTag
        {
            get
            {
                return this._ParentTag.Entity;
            }
            set
            {
                Tag entity = this._ParentTag.Entity;
                if (entity == value && this._ParentTag.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._ParentTag.Entity = (Tag)null;
                    entity.ParentTags.Remove(this);
                }
                this._ParentTag.Entity = value;
                if (value != null)
                {
                    value.ParentTags.Add(this);
                    this._ParentTagId = new int?(value.TagId);
                }
                else
                    this._ParentTagId = new int?();
            }
        }

        [BeterColumn(Internal = false, Unique = true)]
        [Column(CanBeNull = false, DbType = "varchar(150)", Storage = "_TagName", UpdateCheck = UpdateCheck.Never)]
        public string TagName
        {
            get
            {
                return this._TagName;
            }
            set
            {
                this._TagName = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "varchar(250)", Storage = "_TagDescription", UpdateCheck = UpdateCheck.Never)]
        public string TagDescription
        {
            get
            {
                return this._TagDescription;
            }
            set
            {
                this._TagDescription = value;
            }
        }

        [BeterColumn(Description = " ", Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "bit", Storage = "_ExcludeFromFileDirectory", UpdateCheck = UpdateCheck.Never)]
        public bool? ExcludeFromFileDirectory
        {
            get
            {
                return this._ExcludeFromFileDirectory;
            }
            set
            {
                this._ExcludeFromFileDirectory = value;
            }
        }

        [Column(CanBeNull = true, DbType = "int", Storage = "_UserId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = false)]
        public int? UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                int? nullable1 = this._UserId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._User.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._UserId = value;
            }
        }

        [BeterColumn(Description = "User who created the entry in the table", Internal = true, Unique = false)]
        [Association(IsForeignKey = true, OtherKey = "UserId", Storage = "_User", ThisKey = "UserId")]
        public User User
        {
            get
            {
                return this._User.Entity;
            }
            set
            {
                User entity = this._User.Entity;
                if (entity == value && this._User.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._User.Entity = (User)null;
                    entity.Tags.Remove(this);
                }
                this._User.Entity = value;
                if (value != null)
                {
                    value.Tags.Add(this);
                    this._UserId = new int?(value.UserId);
                }
                else
                    this._UserId = new int?();
            }
        }

        [BeterColumn(Internal = true, Unique = true, UpdateUnique = true)]
        [Column(CanBeNull = false, Storage = "_Active", UpdateCheck = UpdateCheck.Never)]
        public bool Active
        {
            get
            {
                return this._Active;
            }
        }

        [Column(CanBeNull = true, Storage = "_DeActivatedOn", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(IgnoreUniqueUpdateCheck = true, Internal = true, Unique = true, UpdateUnique = true)]
        public DateTime? DeActivatedOn
        {
            get
            {
                return this._DeActivatedOn;
            }
        }

        [Association(OtherKey = "TagId", Storage = "_BrokerTags", ThisKey = "TagId")]
        public EntitySet<BrokerTag> BrokerTags
        {
            get
            {
                return this._BrokerTags;
            }
        }

        [Association(OtherKey = "TagId", Storage = "_Directories", ThisKey = "TagId")]
        public EntitySet<Directory> Directories
        {
            get
            {
                return this._Directories;
            }
        }

        [Association(OtherKey = "TagId", Storage = "_FileTags", ThisKey = "TagId")]
        public EntitySet<FileTag> FileTags
        {
            get
            {
                return this._FileTags;
            }
        }

        [Association(OtherKey = "TagId", Storage = "_InboundPipeTags", ThisKey = "TagId")]
        public EntitySet<InboundPipeTag> InboundPipeTags
        {
            get
            {
                return this._InboundPipeTags;
            }
        }

        [Association(OtherKey = "TagId", Storage = "_OutboundInterfaceTags", ThisKey = "TagId")]
        public EntitySet<OutboundInterfaceTag> OutboundInterfaceTags
        {
            get
            {
                return this._OutboundInterfaceTags;
            }
        }

        [Association(OtherKey = "ParentTagId", Storage = "_ParentTags", ThisKey = "TagId")]
        public EntitySet<Tag> ParentTags
        {
            get
            {
                return this._ParentTags;
            }
        }

        public Tag()
        {
        }

        public Tag(DateTime TimeStamp, string TagName)
        {
            this.TimeStamp = TimeStamp;
            this.TagName = TagName;
        }

        public void DeActivate(DateTime deActivatedOn)
        {
            this._Active = false;
            this._DeActivatedOn = new DateTime?(deActivatedOn);
        }

        public void Activate()
        {
            this._Active = true;
            this._DeActivatedOn = new DateTime?();
        }

        public override string ToString()
        {
            return this.TagName.ToString();
        }
    }

    [BeterTable(TrackHistory = true)]
    [Table]
    public class Team
    {
        private bool _Active = true;
        private EntitySet<TeamBroker> _TeamBrokers = new EntitySet<TeamBroker>();
        private EntitySet<TeamMember> _TeamMembers = new EntitySet<TeamMember>();
        private int _TeamId;
        private DateTime _TimeStamp;
        private string _TeamName;
        private string _TeamDescription;
        private int _OwnerId;
        private EntityRef<User> _Owner;
        private DateTime? _DeActivatedOn;
        private int? _UserId;
        private EntityRef<User> _User;
        private string _TeamEmail;

        [Column(IsDbGenerated = true, IsPrimaryKey = true, Storage = "_TeamId")]
        public int TeamId
        {
            get
            {
                return this._TeamId;
            }
            set
            {
                if (this._TeamId == value)
                    return;
                this._TeamId = value;
            }
        }

        [BeterColumn(Description = "When a record was inserted or last updated", Internal = true, Unique = false)]
        [Column(CanBeNull = false, DbType = "datetime", Storage = "_TimeStamp", UpdateCheck = UpdateCheck.Never)]
        public DateTime TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
            }
        }

        [BeterColumn(Internal = false, Unique = true)]
        [Column(CanBeNull = false, DbType = "varchar(50)", Storage = "_TeamName", UpdateCheck = UpdateCheck.Never)]
        public string TeamName
        {
            get
            {
                return this._TeamName;
            }
            set
            {
                this._TeamName = value;
            }
        }

        [Column(CanBeNull = true, DbType = "varchar(250)", Storage = "_TeamDescription", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public string TeamDescription
        {
            get
            {
                return this._TeamDescription;
            }
            set
            {
                this._TeamDescription = value;
            }
        }

        [Column(CanBeNull = false, DbType = "int", Storage = "_OwnerId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public int OwnerId
        {
            get
            {
                return this._OwnerId;
            }
            set
            {
                if (this._OwnerId == value)
                    return;
                if (this._Owner.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._OwnerId = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Association(IsForeignKey = true, OtherKey = "UserId", Storage = "_Owner", ThisKey = "OwnerId")]
        public User Owner
        {
            get
            {
                return this._Owner.Entity;
            }
            set
            {
                User entity = this._Owner.Entity;
                if (entity == value && this._Owner.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._Owner.Entity = (User)null;
                    entity.OwnerTeams.Remove(this);
                }
                this._Owner.Entity = value;
                if (value == null)
                    throw new Exception("'Owner' is a mandatory field and cannot be set to null.");
                value.OwnerTeams.Add(this);
                this._OwnerId = value.UserId;
            }
        }

        [BeterColumn(Internal = true, Unique = true, UpdateUnique = true)]
        [Column(CanBeNull = false, Storage = "_Active", UpdateCheck = UpdateCheck.Never)]
        public bool Active
        {
            get
            {
                return this._Active;
            }
        }

        [Column(CanBeNull = true, Storage = "_DeActivatedOn", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(IgnoreUniqueUpdateCheck = true, Internal = true, Unique = true, UpdateUnique = true)]
        public DateTime? DeActivatedOn
        {
            get
            {
                return this._DeActivatedOn;
            }
        }

        [BeterColumn(Internal = true, Unique = false)]
        [Column(CanBeNull = true, DbType = "int", Storage = "_UserId", UpdateCheck = UpdateCheck.Never)]
        public int? UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                int? nullable1 = this._UserId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._User.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._UserId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "UserId", Storage = "_User", ThisKey = "UserId")]
        [BeterColumn(Description = "User who created the entry in the table", Internal = true, Unique = false)]
        public User User
        {
            get
            {
                return this._User.Entity;
            }
            set
            {
                User entity = this._User.Entity;
                if (entity == value && this._User.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._User.Entity = (User)null;
                    entity.Teams.Remove(this);
                }
                this._User.Entity = value;
                if (value != null)
                {
                    value.Teams.Add(this);
                    this._UserId = new int?(value.UserId);
                }
                else
                    this._UserId = new int?();
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "varchar(150)", Storage = "_TeamEmail", UpdateCheck = UpdateCheck.Never)]
        public string TeamEmail
        {
            get
            {
                return this._TeamEmail;
            }
            set
            {
                this._TeamEmail = value;
            }
        }

        [Association(OtherKey = "TeamId", Storage = "_TeamBrokers", ThisKey = "TeamId")]
        public EntitySet<TeamBroker> TeamBrokers
        {
            get
            {
                return this._TeamBrokers;
            }
        }

        [Association(OtherKey = "TeamId", Storage = "_TeamMembers", ThisKey = "TeamId")]
        public EntitySet<TeamMember> TeamMembers
        {
            get
            {
                return this._TeamMembers;
            }
        }

        public Team()
        {
        }

        public Team(DateTime TimeStamp, string TeamName, User Owner)
        {
            this.TimeStamp = TimeStamp;
            this.TeamName = TeamName;
            this.Owner = Owner;
        }

        public void DeActivate(DateTime deActivatedOn)
        {
            this._Active = false;
            this._DeActivatedOn = new DateTime?(deActivatedOn);
        }

        public void Activate()
        {
            this._Active = true;
            this._DeActivatedOn = new DateTime?();
        }

        public override string ToString()
        {
            return this.TeamName.ToString();
        }
    }

    [Table]
    [BeterTable(TrackHistory = true)]
    public class TeamBroker : IBase
    {
        private bool _Active = true;
        private int _TeamBrokerId;
        private DateTime _TimeStamp;
        private int _TeamId;
        private EntityRef<Team> _Team;
        private int _BrokerId;
        private EntityRef<Broker> _Broker;
        private int? _UserId;
        private EntityRef<User> _User;
        private DateTime? _DeActivatedOn;

        [Column(IsDbGenerated = true, IsPrimaryKey = true, Storage = "_TeamBrokerId")]
        public int TeamBrokerId
        {
            get
            {
                return this._TeamBrokerId;
            }
            set
            {
                if (this._TeamBrokerId == value)
                    return;
                this._TeamBrokerId = value;
            }
        }

        [Column(CanBeNull = false, DbType = "datetime", Storage = "_TimeStamp", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Description = "When a record was inserted or last updated", Internal = true, Unique = false)]
        public DateTime TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
            }
        }

        [BeterColumn(Internal = false, Unique = true)]
        [Column(CanBeNull = false, DbType = "int", Storage = "_TeamId", UpdateCheck = UpdateCheck.Never)]
        public int TeamId
        {
            get
            {
                return this._TeamId;
            }
            set
            {
                if (this._TeamId == value)
                    return;
                if (this._Team.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._TeamId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "TeamId", Storage = "_Team", ThisKey = "TeamId")]
        [BeterColumn(Internal = false, Unique = true)]
        public Team Team
        {
            get
            {
                return this._Team.Entity;
            }
            set
            {
                Team entity = this._Team.Entity;
                if (entity == value && this._Team.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._Team.Entity = (Team)null;
                    entity.TeamBrokers.Remove(this);
                }
                this._Team.Entity = value;
                if (value == null)
                    throw new Exception("'Team' is a mandatory field and cannot be set to null.");
                value.TeamBrokers.Add(this);
                this._TeamId = value.TeamId;
            }
        }

        [BeterColumn(Internal = false, Unique = true)]
        [Column(CanBeNull = false, DbType = "int", Storage = "_BrokerId", UpdateCheck = UpdateCheck.Never)]
        public int BrokerId
        {
            get
            {
                return this._BrokerId;
            }
            set
            {
                if (this._BrokerId == value)
                    return;
                if (this._Broker.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._BrokerId = value;
            }
        }

        [BeterColumn(Internal = false, Unique = true)]
        [Association(IsForeignKey = true, OtherKey = "BrokerId", Storage = "_Broker", ThisKey = "BrokerId")]
        public Broker Broker
        {
            get
            {
                return this._Broker.Entity;
            }
            set
            {
                Broker entity = this._Broker.Entity;
                if (entity == value && this._Broker.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._Broker.Entity = (Broker)null;
                    entity.TeamBrokers.Remove(this);
                }
                this._Broker.Entity = value;
                if (value == null)
                    throw new Exception("'Broker' is a mandatory field and cannot be set to null.");
                value.TeamBrokers.Add(this);
                this._BrokerId = value.BrokerId;
            }
        }

        [BeterColumn(Internal = true, Unique = false)]
        [Column(CanBeNull = true, DbType = "int", Storage = "_UserId", UpdateCheck = UpdateCheck.Never)]
        public int? UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                int? nullable1 = this._UserId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._User.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._UserId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "UserId", Storage = "_User", ThisKey = "UserId")]
        [BeterColumn(Description = "User who created the entry in the table", Internal = true, Unique = false)]
        public User User
        {
            get
            {
                return this._User.Entity;
            }
            set
            {
                User entity = this._User.Entity;
                if (entity == value && this._User.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._User.Entity = (User)null;
                    entity.TeamBrokers.Remove(this);
                }
                this._User.Entity = value;
                if (value != null)
                {
                    value.TeamBrokers.Add(this);
                    this._UserId = new int?(value.UserId);
                }
                else
                    this._UserId = new int?();
            }
        }

        [BeterColumn(Internal = true, Unique = true, UpdateUnique = true)]
        [Column(CanBeNull = false, Storage = "_Active", UpdateCheck = UpdateCheck.Never)]
        public bool Active
        {
            get
            {
                return this._Active;
            }
        }

        [Column(CanBeNull = true, Storage = "_DeActivatedOn", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(IgnoreUniqueUpdateCheck = true, Internal = true, Unique = true, UpdateUnique = true)]
        public DateTime? DeActivatedOn
        {
            get
            {
                return this._DeActivatedOn;
            }
        }

        public TeamBroker()
        {
        }

        public TeamBroker(DateTime TimeStamp, Team Team, Broker Broker)
        {
            this.TimeStamp = TimeStamp;
            this.Team = Team;
            this.Broker = Broker;
        }

        public void DeActivate(DateTime deActivatedOn)
        {
            this._Active = false;
            this._DeActivatedOn = new DateTime?(deActivatedOn);
        }

        public void Activate()
        {
            this._Active = true;
            this._DeActivatedOn = new DateTime?();
        }

        public override string ToString()
        {
            return this.TeamId.ToString() + " | " + this.BrokerId.ToString();
        }
    }

    [Table]
    [BeterTable(TrackHistory = true)]
    public class TeamMember
    {
        private bool _Active = true;
        private int _TeamMemberId;
        private DateTime _TimeStamp;
        private int _TeamId;
        private EntityRef<Team> _Team;
        private int _MemberId;
        private EntityRef<User> _Member;
        private int? _UserId;
        private EntityRef<User> _User;
        private DateTime? _DeActivatedOn;

        [Column(IsDbGenerated = true, IsPrimaryKey = true, Storage = "_TeamMemberId")]
        public int TeamMemberId
        {
            get
            {
                return this._TeamMemberId;
            }
            set
            {
                if (this._TeamMemberId == value)
                    return;
                this._TeamMemberId = value;
            }
        }

        [Column(CanBeNull = false, DbType = "datetime", Storage = "_TimeStamp", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Description = "When a record was inserted or last updated", Internal = true, Unique = false)]
        public DateTime TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
            }
        }

        [Column(CanBeNull = false, DbType = "int", Storage = "_TeamId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = true)]
        public int TeamId
        {
            get
            {
                return this._TeamId;
            }
            set
            {
                if (this._TeamId == value)
                    return;
                if (this._Team.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._TeamId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "TeamId", Storage = "_Team", ThisKey = "TeamId")]
        [BeterColumn(Internal = false, Unique = true)]
        public Team Team
        {
            get
            {
                return this._Team.Entity;
            }
            set
            {
                Team entity = this._Team.Entity;
                if (entity == value && this._Team.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._Team.Entity = (Team)null;
                    entity.TeamMembers.Remove(this);
                }
                this._Team.Entity = value;
                if (value == null)
                    throw new Exception("'Team' is a mandatory field and cannot be set to null.");
                value.TeamMembers.Add(this);
                this._TeamId = value.TeamId;
            }
        }

        [BeterColumn(Internal = false, Unique = true)]
        [Column(CanBeNull = false, DbType = "int", Storage = "_MemberId", UpdateCheck = UpdateCheck.Never)]
        public int MemberId
        {
            get
            {
                return this._MemberId;
            }
            set
            {
                if (this._MemberId == value)
                    return;
                if (this._Member.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._MemberId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "UserId", Storage = "_Member", ThisKey = "MemberId")]
        [BeterColumn(Internal = false, Unique = true)]
        public User Member
        {
            get
            {
                return this._Member.Entity;
            }
            set
            {
                User entity = this._Member.Entity;
                if (entity == value && this._Member.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._Member.Entity = (User)null;
                    entity.MemberTeamMembers.Remove(this);
                }
                this._Member.Entity = value;
                if (value == null)
                    throw new Exception("'Member' is a mandatory field and cannot be set to null.");
                value.MemberTeamMembers.Add(this);
                this._MemberId = value.UserId;
            }
        }

        [Column(CanBeNull = true, DbType = "int", Storage = "_UserId", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = false)]
        public int? UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                int? nullable1 = this._UserId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._User.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._UserId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "UserId", Storage = "_User", ThisKey = "UserId")]
        [BeterColumn(Description = "User who created the entry in the table", Internal = true, Unique = false)]
        public User User
        {
            get
            {
                return this._User.Entity;
            }
            set
            {
                User entity = this._User.Entity;
                if (entity == value && this._User.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._User.Entity = (User)null;
                    entity.TeamMembers.Remove(this);
                }
                this._User.Entity = value;
                if (value != null)
                {
                    value.TeamMembers.Add(this);
                    this._UserId = new int?(value.UserId);
                }
                else
                    this._UserId = new int?();
            }
        }

        [BeterColumn(Internal = true, Unique = true, UpdateUnique = true)]
        [Column(CanBeNull = false, Storage = "_Active", UpdateCheck = UpdateCheck.Never)]
        public bool Active
        {
            get
            {
                return this._Active;
            }
        }

        [Column(CanBeNull = true, Storage = "_DeActivatedOn", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(IgnoreUniqueUpdateCheck = true, Internal = true, Unique = true, UpdateUnique = true)]
        public DateTime? DeActivatedOn
        {
            get
            {
                return this._DeActivatedOn;
            }
        }

        public TeamMember()
        {
        }

        public TeamMember(DateTime TimeStamp, Team Team, User Member)
        {
            this.TimeStamp = TimeStamp;
            this.Team = Team;
            this.Member = Member;
        }

        public void DeActivate(DateTime deActivatedOn)
        {
            this._Active = false;
            this._DeActivatedOn = new DateTime?(deActivatedOn);
        }

        public void Activate()
        {
            this._Active = true;
            this._DeActivatedOn = new DateTime?();
        }

        public override string ToString()
        {
            return this.TeamId.ToString() + " | " + this.MemberId.ToString();
        }
    }

    [BeterTable(Description = "Raw text, delimited formats like CSV, EDIFACT, X12, etc.", TrackHistory = true)]
    [Table]
    public class TextFile : IBase
    {
        private bool _Active = true;
        private int _TextFileId;
        private DateTime _TimeStamp;
        private int _FileId;
        private EntityRef<File> _File;
        private string _TextContent;
        private int? _UserId;
        private EntityRef<User> _User;
        private DateTime? _DeActivatedOn;

        [Column(IsDbGenerated = true, IsPrimaryKey = true, Storage = "_TextFileId")]
        public int TextFileId
        {
            get
            {
                return this._TextFileId;
            }
            set
            {
                if (this._TextFileId == value)
                    return;
                this._TextFileId = value;
            }
        }

        [BeterColumn(Description = "When a record was inserted or last updated", Internal = true, Unique = false)]
        [Column(CanBeNull = false, DbType = "datetime", Storage = "_TimeStamp", UpdateCheck = UpdateCheck.Never)]
        public DateTime TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = false, DbType = "int", Storage = "_FileId", UpdateCheck = UpdateCheck.Never)]
        public int FileId
        {
            get
            {
                return this._FileId;
            }
            set
            {
                if (this._FileId == value)
                    return;
                if (this._File.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._FileId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "FileId", Storage = "_File", ThisKey = "FileId")]
        [BeterColumn(Internal = false, Unique = false)]
        public File File
        {
            get
            {
                return this._File.Entity;
            }
            set
            {
                File entity = this._File.Entity;
                if (entity == value && this._File.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._File.Entity = (File)null;
                    entity.TextFiles.Remove(this);
                }
                this._File.Entity = value;
                if (value == null)
                    throw new Exception("'File' is a mandatory field and cannot be set to null.");
                value.TextFiles.Add(this);
                this._FileId = value.FileId;
            }
        }

        [Column(CanBeNull = true, DbType = "varchar(max)", Storage = "_TextContent", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = false, Unique = false)]
        public string TextContent
        {
            get
            {
                return this._TextContent;
            }
            set
            {
                this._TextContent = value;
            }
        }

        [BeterColumn(Internal = true, Unique = false)]
        [Column(CanBeNull = true, DbType = "int", Storage = "_UserId", UpdateCheck = UpdateCheck.Never)]
        public int? UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                int? nullable1 = this._UserId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._User.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._UserId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "UserId", Storage = "_User", ThisKey = "UserId")]
        [BeterColumn(Description = "User who created the entry in the table", Internal = true, Unique = false)]
        public User User
        {
            get
            {
                return this._User.Entity;
            }
            set
            {
                User entity = this._User.Entity;
                if (entity == value && this._User.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._User.Entity = (User)null;
                    entity.TextFiles.Remove(this);
                }
                this._User.Entity = value;
                if (value != null)
                {
                    value.TextFiles.Add(this);
                    this._UserId = new int?(value.UserId);
                }
                else
                    this._UserId = new int?();
            }
        }

        [BeterColumn(Internal = true, Unique = false)]
        [Column(CanBeNull = false, Storage = "_Active", UpdateCheck = UpdateCheck.Never)]
        public bool Active
        {
            get
            {
                return this._Active;
            }
        }

        [BeterColumn(Internal = true, Unique = false)]
        [Column(CanBeNull = true, Storage = "_DeActivatedOn", UpdateCheck = UpdateCheck.Never)]
        public DateTime? DeActivatedOn
        {
            get
            {
                return this._DeActivatedOn;
            }
        }

        public TextFile()
        {
        }

        public TextFile(DateTime TimeStamp, File File)
        {
            this.TimeStamp = TimeStamp;
            this.File = File;
        }

        public void DeActivate(DateTime deActivatedOn)
        {
            this._Active = false;
            this._DeActivatedOn = new DateTime?(deActivatedOn);
        }

        public void Activate()
        {
            this._Active = true;
            this._DeActivatedOn = new DateTime?();
        }

        public override string ToString()
        {
            return this.FileId.ToString();
        }
    }

    [Table]
    [BeterTable(TrackHistory = true)]
    public class User
    {
        private bool _Active = true;
        private EntitySet<InterfaceRequest> _InterfaceRequests = new EntitySet<InterfaceRequest>();
        private EntitySet<As2Connection> _As2Connections = new EntitySet<As2Connection>();
        private EntitySet<As2Log> _As2Logs = new EntitySet<As2Log>();
        private EntitySet<As2Pipe> _AS2Pipes = new EntitySet<As2Pipe>();
        private EntitySet<BinaryFile> _BinaryFiles = new EntitySet<BinaryFile>();
        private EntitySet<Broker> _Brokers = new EntitySet<Broker>();
        private EntitySet<BrokerLink> _BrokerLinks = new EntitySet<BrokerLink>();
        private EntitySet<BrokerTag> _BrokerTags = new EntitySet<BrokerTag>();
        private EntitySet<Connection> _Connections = new EntitySet<Connection>();
        private EntitySet<Directory> _Directories = new EntitySet<Directory>();
        private EntitySet<EmailConnection> _EmailConnections = new EntitySet<EmailConnection>();
        private EntitySet<EmailFile> _EmailFiles = new EntitySet<EmailFile>();
        private EntitySet<EmailPipe> _EmailPipes = new EntitySet<EmailPipe>();
        private EntitySet<Execution> _Executions = new EntitySet<Execution>();
        private EntitySet<ExecutionLog> _ExecutionLogs = new EntitySet<ExecutionLog>();
        private EntitySet<File> _Files = new EntitySet<File>();
        private EntitySet<FileDirectory> _FileDirectories = new EntitySet<FileDirectory>();
        private EntitySet<FileTag> _FileTags = new EntitySet<FileTag>();
        private EntitySet<FtpConnection> _FtpConnections = new EntitySet<FtpConnection>();
        private EntitySet<FtpLog> _FtpLogs = new EntitySet<FtpLog>();
        private EntitySet<FtpPipe> _FTPPipes = new EntitySet<FtpPipe>();
        private EntitySet<HttpPipe> _HttpPipes = new EntitySet<HttpPipe>();
        private EntitySet<InboundPipeTag> _InboundPipeTags = new EntitySet<InboundPipeTag>();
        private EntitySet<InboundPipeTagSortRule> _InboundPipeTagSortRules = new EntitySet<InboundPipeTagSortRule>();
        private EntitySet<Interface> _Interfaces = new EntitySet<Interface>();
        private EntitySet<InterfaceSortRule> _InterfaceSortRules = new EntitySet<InterfaceSortRule>();
        private EntitySet<Library> _Libraries = new EntitySet<Library>();
        private EntitySet<LibraryVersion> _LibraryVersions = new EntitySet<LibraryVersion>();
        private EntitySet<Log> _Logs = new EntitySet<Log>();
        private EntitySet<OutboundInterfaceTag> _OutboundInterfaceTags = new EntitySet<OutboundInterfaceTag>();
        private EntitySet<OutboundPipeSortRule> _OutboundPipeSortRules = new EntitySet<OutboundPipeSortRule>();
        private EntitySet<OutboundTask> _OutboundTasks = new EntitySet<OutboundTask>();
        private EntitySet<Pipeline> _Pipelines = new EntitySet<Pipeline>();
        private EntitySet<ProcessBlock> _ProcessBlocks = new EntitySet<ProcessBlock>();
        private EntitySet<ProcessTask> _ProcessTasks = new EntitySet<ProcessTask>();
        private EntitySet<SecurityPool> _SecurityPools = new EntitySet<SecurityPool>();
        private EntitySet<Tag> _Tags = new EntitySet<Tag>();
        private EntitySet<Team> _Teams = new EntitySet<Team>();
        private EntitySet<Team> _OwnerTeams = new EntitySet<Team>();
        private EntitySet<TeamBroker> _TeamBrokers = new EntitySet<TeamBroker>();
        private EntitySet<TeamMember> _TeamMembers = new EntitySet<TeamMember>();
        private EntitySet<TeamMember> _MemberTeamMembers = new EntitySet<TeamMember>();
        private EntitySet<TextFile> _TextFiles = new EntitySet<TextFile>();
        private EntitySet<XmlFile> _XmlFiles = new EntitySet<XmlFile>();
        private EntitySet<FileExecution> _FileExecutions = new EntitySet<FileExecution>();
        private int _UserId;
        private DateTime _TimeStamp;
        private string _Username;
        private string _UserDisplayName;
        private DateTime? _DeActivatedOn;
        private string _UserEmail;

        [Column(IsDbGenerated = true, IsPrimaryKey = true, Storage = "_UserId")]
        public int UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                if (this._UserId == value)
                    return;
                this._UserId = value;
            }
        }

        [BeterColumn(Description = "When a record was inserted or last updated", Internal = true, Unique = false)]
        [Column(CanBeNull = false, DbType = "datetime", Storage = "_TimeStamp", UpdateCheck = UpdateCheck.Never)]
        public DateTime TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
            }
        }

        [BeterColumn(Internal = false, Unique = true)]
        [Column(CanBeNull = false, DbType = "varchar(50)", Storage = "_Username", UpdateCheck = UpdateCheck.Never)]
        public string Username
        {
            get
            {
                return this._Username;
            }
            set
            {
                this._Username = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "varchar(50)", Storage = "_UserDisplayName", UpdateCheck = UpdateCheck.Never)]
        public string UserDisplayName
        {
            get
            {
                return this._UserDisplayName;
            }
            set
            {
                this._UserDisplayName = value;
            }
        }

        [BeterColumn(Internal = true, Unique = true, UpdateUnique = true)]
        [Column(CanBeNull = false, Storage = "_Active", UpdateCheck = UpdateCheck.Never)]
        public bool Active
        {
            get
            {
                return this._Active;
            }
        }

        [Column(CanBeNull = true, Storage = "_DeActivatedOn", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(IgnoreUniqueUpdateCheck = true, Internal = true, Unique = true, UpdateUnique = true)]
        public DateTime? DeActivatedOn
        {
            get
            {
                return this._DeActivatedOn;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = false, DbType = "varchar(150)", Storage = "_UserEmail", UpdateCheck = UpdateCheck.Never)]
        public string UserEmail
        {
            get
            {
                return this._UserEmail;
            }
            set
            {
                this._UserEmail = value;
            }
        }

        [Association(OtherKey = "UserId", Storage = "_InterfaceRequests", ThisKey = "UserId")]
        public EntitySet<InterfaceRequest> InterfaceRequests
        {
            get
            {
                return this._InterfaceRequests;
            }
        }

        [Association(OtherKey = "UserId", Storage = "_As2Connections", ThisKey = "UserId")]
        public EntitySet<As2Connection> As2Connections
        {
            get
            {
                return this._As2Connections;
            }
        }

        [Association(OtherKey = "UserId", Storage = "_As2Logs", ThisKey = "UserId")]
        public EntitySet<As2Log> As2Logs
        {
            get
            {
                return this._As2Logs;
            }
        }

        [Association(OtherKey = "UserId", Storage = "_AS2Pipes", ThisKey = "UserId")]
        public EntitySet<As2Pipe> AS2Pipes
        {
            get
            {
                return this._AS2Pipes;
            }
        }

        [Association(OtherKey = "UserId", Storage = "_BinaryFiles", ThisKey = "UserId")]
        public EntitySet<BinaryFile> BinaryFiles
        {
            get
            {
                return this._BinaryFiles;
            }
        }

        [Association(OtherKey = "UserId", Storage = "_Brokers", ThisKey = "UserId")]
        public EntitySet<Broker> Brokers
        {
            get
            {
                return this._Brokers;
            }
        }

        [Association(OtherKey = "UserId", Storage = "_BrokerLinks", ThisKey = "UserId")]
        public EntitySet<BrokerLink> BrokerLinks
        {
            get
            {
                return this._BrokerLinks;
            }
        }

        [Association(OtherKey = "UserId", Storage = "_BrokerTags", ThisKey = "UserId")]
        public EntitySet<BrokerTag> BrokerTags
        {
            get
            {
                return this._BrokerTags;
            }
        }

        [Association(OtherKey = "UserId", Storage = "_Connections", ThisKey = "UserId")]
        public EntitySet<Connection> Connections
        {
            get
            {
                return this._Connections;
            }
        }

        [Association(OtherKey = "UserId", Storage = "_Directories", ThisKey = "UserId")]
        public EntitySet<Directory> Directories
        {
            get
            {
                return this._Directories;
            }
        }

        [Association(OtherKey = "UserId", Storage = "_EmailConnections", ThisKey = "UserId")]
        public EntitySet<EmailConnection> EmailConnections
        {
            get
            {
                return this._EmailConnections;
            }
        }

        [Association(OtherKey = "UserId", Storage = "_EmailFiles", ThisKey = "UserId")]
        public EntitySet<EmailFile> EmailFiles
        {
            get
            {
                return this._EmailFiles;
            }
        }

        [Association(OtherKey = "UserId", Storage = "_EmailPipes", ThisKey = "UserId")]
        public EntitySet<EmailPipe> EmailPipes
        {
            get
            {
                return this._EmailPipes;
            }
        }

        [Association(OtherKey = "UserId", Storage = "_Executions", ThisKey = "UserId")]
        public EntitySet<Execution> Executions
        {
            get
            {
                return this._Executions;
            }
        }

        [Association(OtherKey = "UserId", Storage = "_ExecutionLogs", ThisKey = "UserId")]
        public EntitySet<ExecutionLog> ExecutionLogs
        {
            get
            {
                return this._ExecutionLogs;
            }
        }

        [Association(OtherKey = "UserId", Storage = "_Files", ThisKey = "UserId")]
        public EntitySet<File> Files
        {
            get
            {
                return this._Files;
            }
        }

        [Association(OtherKey = "UserId", Storage = "_FileDirectories", ThisKey = "UserId")]
        public EntitySet<FileDirectory> FileDirectories
        {
            get
            {
                return this._FileDirectories;
            }
        }

        [Association(OtherKey = "UserId", Storage = "_FileTags", ThisKey = "UserId")]
        public EntitySet<FileTag> FileTags
        {
            get
            {
                return this._FileTags;
            }
        }

        [Association(OtherKey = "UserId", Storage = "_FtpConnections", ThisKey = "UserId")]
        public EntitySet<FtpConnection> FtpConnections
        {
            get
            {
                return this._FtpConnections;
            }
        }

        [Association(OtherKey = "UserId", Storage = "_FtpLogs", ThisKey = "UserId")]
        public EntitySet<FtpLog> FtpLogs
        {
            get
            {
                return this._FtpLogs;
            }
        }

        [Association(OtherKey = "UserId", Storage = "_FTPPipes", ThisKey = "UserId")]
        public EntitySet<FtpPipe> FTPPipes
        {
            get
            {
                return this._FTPPipes;
            }
        }

        [Association(OtherKey = "UserId", Storage = "_HttpPipes", ThisKey = "UserId")]
        public EntitySet<HttpPipe> HttpPipes
        {
            get
            {
                return this._HttpPipes;
            }
        }

        [Association(OtherKey = "UserId", Storage = "_InboundPipeTags", ThisKey = "UserId")]
        public EntitySet<InboundPipeTag> InboundPipeTags
        {
            get
            {
                return this._InboundPipeTags;
            }
        }

        [Association(OtherKey = "UserId", Storage = "_InboundPipeTagSortRules", ThisKey = "UserId")]
        public EntitySet<InboundPipeTagSortRule> InboundPipeTagSortRules
        {
            get
            {
                return this._InboundPipeTagSortRules;
            }
        }

        [Association(OtherKey = "UserId", Storage = "_Interfaces", ThisKey = "UserId")]
        public EntitySet<Interface> Interfaces
        {
            get
            {
                return this._Interfaces;
            }
        }

        [Association(OtherKey = "UserId", Storage = "_InterfaceSortRules", ThisKey = "UserId")]
        public EntitySet<InterfaceSortRule> InterfaceSortRules
        {
            get
            {
                return this._InterfaceSortRules;
            }
        }

        [Association(OtherKey = "UserId", Storage = "_Libraries", ThisKey = "UserId")]
        public EntitySet<Library> Libraries
        {
            get
            {
                return this._Libraries;
            }
        }

        [Association(OtherKey = "UserId", Storage = "_LibraryVersions", ThisKey = "UserId")]
        public EntitySet<LibraryVersion> LibraryVersions
        {
            get
            {
                return this._LibraryVersions;
            }
        }

        [Association(OtherKey = "UserId", Storage = "_Logs", ThisKey = "UserId")]
        public EntitySet<Log> Logs
        {
            get
            {
                return this._Logs;
            }
        }

        [Association(OtherKey = "UserId", Storage = "_OutboundInterfaceTags", ThisKey = "UserId")]
        public EntitySet<OutboundInterfaceTag> OutboundInterfaceTags
        {
            get
            {
                return this._OutboundInterfaceTags;
            }
        }

        [Association(OtherKey = "UserId", Storage = "_OutboundPipeSortRules", ThisKey = "UserId")]
        public EntitySet<OutboundPipeSortRule> OutboundPipeSortRules
        {
            get
            {
                return this._OutboundPipeSortRules;
            }
        }

        [Association(OtherKey = "UserId", Storage = "_OutboundTasks", ThisKey = "UserId")]
        public EntitySet<OutboundTask> OutboundTasks
        {
            get
            {
                return this._OutboundTasks;
            }
        }

        [Association(OtherKey = "UserId", Storage = "_Pipelines", ThisKey = "UserId")]
        public EntitySet<Pipeline> Pipelines
        {
            get
            {
                return this._Pipelines;
            }
        }

        [Association(OtherKey = "UserId", Storage = "_ProcessBlocks", ThisKey = "UserId")]
        public EntitySet<ProcessBlock> ProcessBlocks
        {
            get
            {
                return this._ProcessBlocks;
            }
        }

        [Association(OtherKey = "UserId", Storage = "_ProcessTasks", ThisKey = "UserId")]
        public EntitySet<ProcessTask> ProcessTasks
        {
            get
            {
                return this._ProcessTasks;
            }
        }

        [Association(OtherKey = "UserId", Storage = "_SecurityPools", ThisKey = "UserId")]
        public EntitySet<SecurityPool> SecurityPools
        {
            get
            {
                return this._SecurityPools;
            }
        }

        [Association(OtherKey = "UserId", Storage = "_Tags", ThisKey = "UserId")]
        public EntitySet<Tag> Tags
        {
            get
            {
                return this._Tags;
            }
        }

        [Association(OtherKey = "UserId", Storage = "_Teams", ThisKey = "UserId")]
        public EntitySet<Team> Teams
        {
            get
            {
                return this._Teams;
            }
        }

        [Association(OtherKey = "OwnerId", Storage = "_OwnerTeams", ThisKey = "UserId")]
        public EntitySet<Team> OwnerTeams
        {
            get
            {
                return this._OwnerTeams;
            }
        }

        [Association(OtherKey = "UserId", Storage = "_TeamBrokers", ThisKey = "UserId")]
        public EntitySet<TeamBroker> TeamBrokers
        {
            get
            {
                return this._TeamBrokers;
            }
        }

        [Association(OtherKey = "UserId", Storage = "_TeamMembers", ThisKey = "UserId")]
        public EntitySet<TeamMember> TeamMembers
        {
            get
            {
                return this._TeamMembers;
            }
        }

        [Association(OtherKey = "MemberId", Storage = "_MemberTeamMembers", ThisKey = "UserId")]
        public EntitySet<TeamMember> MemberTeamMembers
        {
            get
            {
                return this._MemberTeamMembers;
            }
        }

        [Association(OtherKey = "UserId", Storage = "_TextFiles", ThisKey = "UserId")]
        public EntitySet<TextFile> TextFiles
        {
            get
            {
                return this._TextFiles;
            }
        }

        [Association(OtherKey = "UserId", Storage = "_XmlFiles", ThisKey = "UserId")]
        public EntitySet<XmlFile> XmlFiles
        {
            get
            {
                return this._XmlFiles;
            }
        }

        [Association(OtherKey = "UserId", Storage = "_FileExecutions", ThisKey = "UserId")]
        public EntitySet<FileExecution> FileExecutions
        {
            get
            {
                return this._FileExecutions;
            }
        }

        public User()
        {
        }

        public User(DateTime TimeStamp, string Username, string UserEmail)
        {
            this.TimeStamp = TimeStamp;
            this.Username = Username;
            this.UserEmail = UserEmail;
        }

        public void DeActivate(DateTime deActivatedOn)
        {
            this._Active = false;
            this._DeActivatedOn = new DateTime?(deActivatedOn);
        }

        public void Activate()
        {
            this._Active = true;
            this._DeActivatedOn = new DateTime?();
        }

        public override string ToString()
        {
            return this.UserDisplayName.ToString();
        }

        public bool HasPermission(Broker b)
        {
            return Enumerable.Any<TeamMember>((IEnumerable<TeamMember>)this.MemberTeamMembers, (Func<TeamMember, bool>)(n =>
            {
                if (n.Active && n.Team.Active)
                    return Enumerable.Any<TeamBroker>((IEnumerable<TeamBroker>)n.Team.TeamBrokers, (Func<TeamBroker, bool>)(x =>
                    {
                        if (x.Active)
                            return x.BrokerId == b.BrokerId;
                        return false;
                    }));
                return false;
            }));
        }

        public IOrderedEnumerable<Broker> EffectiveBrokers()
        {
            Dictionary<int, Broker> dictionary = new Dictionary<int, Broker>();
            foreach (Team team in Enumerable.Select<TeamMember, Team>(Enumerable.Where<TeamMember>((IEnumerable<TeamMember>)this.MemberTeamMembers, (Func<TeamMember, bool>)(u =>
            {
                if (u.Active)
                    return u.Team.Active;
                return false;
            })), (Func<TeamMember, Team>)(u => u.Team)))
            {
                foreach (Broker broker in Enumerable.Select<TeamBroker, Broker>(Enumerable.Where<TeamBroker>((IEnumerable<TeamBroker>)team.TeamBrokers, (Func<TeamBroker, bool>)(u =>
                {
                    if (u.Active)
                        return u.Broker.Active;
                    return false;
                })), (Func<TeamBroker, Broker>)(u => u.Broker)))
                {
                    if (!dictionary.ContainsKey(broker.BrokerId))
                        dictionary.Add(broker.BrokerId, broker);
                }
            }
            return Enumerable.OrderBy<Broker, string>((IEnumerable<Broker>)dictionary.Values, (Func<Broker, string>)(n => n.BrokerName));
        }

        public IEnumerable<Pipeline> EffectivePipelines()
        {
            List<Pipeline> list = new List<Pipeline>();
            foreach (Broker broker in (IEnumerable<Broker>)this.EffectiveBrokers())
            {
                foreach (Connection connection in (IEnumerable<Connection>)Enumerable.OrderBy<Connection, string>(Enumerable.Where<Connection>((IEnumerable<Connection>)broker.Connections, (Func<Connection, bool>)(n => n.Active)), (Func<Connection, string>)(n => n.ConnectionName)))
                    Enumerable.Union<Pipeline>((IEnumerable<Pipeline>)list, Enumerable.Where<Pipeline>((IEnumerable<Pipeline>)connection.Pipelines, (Func<Pipeline, bool>)(n => n.Active)));
            }
            return (IEnumerable<Pipeline>)list;
        }
    }

    [Table]
    [BeterTable(TrackHistory = true)]
    public class XmlFile : IBase
    {
        private bool _Active = true;
        private int _XmlFileId;
        private DateTime _TimeStamp;
        private int _FileId;
        private EntityRef<File> _File;
        private XDocument _XmlContent;
        private int? _UserId;
        private EntityRef<User> _User;
        private DateTime? _DeActivatedOn;

        [Column(IsDbGenerated = true, IsPrimaryKey = true, Storage = "_XmlFileId")]
        public int XmlFileId
        {
            get
            {
                return this._XmlFileId;
            }
            set
            {
                if (this._XmlFileId == value)
                    return;
                this._XmlFileId = value;
            }
        }

        [BeterColumn(Description = "When a record was inserted or last updated", Internal = true, Unique = false)]
        [Column(CanBeNull = false, DbType = "datetime", Storage = "_TimeStamp", UpdateCheck = UpdateCheck.Never)]
        public DateTime TimeStamp
        {
            get
            {
                return this._TimeStamp;
            }
            set
            {
                this._TimeStamp = value;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = false, DbType = "int", Storage = "_FileId", UpdateCheck = UpdateCheck.Never)]
        public int FileId
        {
            get
            {
                return this._FileId;
            }
            set
            {
                if (this._FileId == value)
                    return;
                if (this._File.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._FileId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "FileId", Storage = "_File", ThisKey = "FileId")]
        [BeterColumn(Internal = false, Unique = false)]
        public File File
        {
            get
            {
                return this._File.Entity;
            }
            set
            {
                File entity = this._File.Entity;
                if (entity == value && this._File.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._File.Entity = (File)null;
                    entity.XmlFiles.Remove(this);
                }
                this._File.Entity = value;
                if (value == null)
                    throw new Exception("'File' is a mandatory field and cannot be set to null.");
                value.XmlFiles.Add(this);
                this._FileId = value.FileId;
            }
        }

        [BeterColumn(Internal = false, Unique = false)]
        [Column(CanBeNull = true, DbType = "xml", Storage = "_XmlContent", UpdateCheck = UpdateCheck.Never)]
        public XDocument XmlContent
        {
            get
            {
                return this._XmlContent;
            }
            set
            {
                this._XmlContent = value;
            }
        }

        [BeterColumn(Internal = true, Unique = false)]
        [Column(CanBeNull = true, DbType = "int", Storage = "_UserId", UpdateCheck = UpdateCheck.Never)]
        public int? UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                int? nullable1 = this._UserId;
                int? nullable2 = value;
                if ((nullable1.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (nullable1.HasValue != nullable2.HasValue ? 1 : 0)) == 0)
                    return;
                if (this._User.HasLoadedOrAssignedValue)
                    throw new ForeignKeyReferenceAlreadyHasValueException();
                this._UserId = value;
            }
        }

        [Association(IsForeignKey = true, OtherKey = "UserId", Storage = "_User", ThisKey = "UserId")]
        [BeterColumn(Description = "User who created the entry in the table", Internal = true, Unique = false)]
        public User User
        {
            get
            {
                return this._User.Entity;
            }
            set
            {
                User entity = this._User.Entity;
                if (entity == value && this._User.HasLoadedOrAssignedValue)
                    return;
                if (entity != null)
                {
                    this._User.Entity = (User)null;
                    entity.XmlFiles.Remove(this);
                }
                this._User.Entity = value;
                if (value != null)
                {
                    value.XmlFiles.Add(this);
                    this._UserId = new int?(value.UserId);
                }
                else
                    this._UserId = new int?();
            }
        }

        [BeterColumn(Internal = true, Unique = false)]
        [Column(CanBeNull = false, Storage = "_Active", UpdateCheck = UpdateCheck.Never)]
        public bool Active
        {
            get
            {
                return this._Active;
            }
        }

        [Column(CanBeNull = true, Storage = "_DeActivatedOn", UpdateCheck = UpdateCheck.Never)]
        [BeterColumn(Internal = true, Unique = false)]
        public DateTime? DeActivatedOn
        {
            get
            {
                return this._DeActivatedOn;
            }
        }

        public XmlFile()
        {
        }

        public XmlFile(DateTime TimeStamp, File File)
        {
            this.TimeStamp = TimeStamp;
            this.File = File;
        }

        public void DeActivate(DateTime deActivatedOn)
        {
            this._Active = false;
            this._DeActivatedOn = new DateTime?(deActivatedOn);
        }

        public void Activate()
        {
            this._Active = true;
            this._DeActivatedOn = new DateTime?();
        }

        public override string ToString()
        {
            return this.FileId.ToString();
        }
    }

    public class XTag
    {
        private List<XTag> _tags;

        public string TagName { get; set; }

        public bool ShowFile { get; set; }

        public List<XTag> Tags
        {
            get
            {
                return this._tags;
            }
        }

        public XTag(string name, bool showFile = true)
        {
            this.TagName = name;
            this.ShowFile = showFile;
            this._tags = new List<XTag>();
        }

        public XTag(string name, XTag childTag, bool showFile = false)
        {
            this.TagName = name;
            this.ShowFile = showFile;
            this._tags = new List<XTag>();
            this._tags.Add(childTag);
        }

        public XTag(string name, params XTag[] childTags)
        {
            this.TagName = name;
            this.ShowFile = false;
            this._tags = new List<XTag>();
            this._tags.AddRange((IEnumerable<XTag>)childTags);
        }

        public void Add(XTag tag)
        {
            this._tags.Add(tag);
        }
    }
}
