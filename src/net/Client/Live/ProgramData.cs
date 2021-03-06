﻿// Copyright 2012 Microsoft Corporation
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Data.Services.Common;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.WindowsAzure.MediaServices.Client
{
    [DataServiceKey("Id")]
    internal class ProgramData : RestEntity<ProgramData>, IProgram
    {
        private IChannel _channel;
        private IAsset _asset;

        protected override string EntitySetName { get { return ProgramBaseCollection.ProgramSet; } }

        /// <summary>
        /// Gets or sets name of the program.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets description of the program.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets program creation date.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets program last modification date.
        /// </summary>
        public DateTime LastModified { get; set; }

        /// <summary>
        /// Gets or sets id of the channel containing the program.
        /// </summary>
        public string ChannelId { get; set; }

        /// <summary>
        /// Gets or sets id of the asset for storing channel content.
        /// </summary>
        public string AssetId { get; set; }

        /// <summary>
        /// Gets or sets the streaming manifest file name. 
        /// </summary>
        public string ManifestName { get; set; }

        /// <summary>
        /// Gets or sets program state.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Gets program state.
        /// </summary>
        ProgramState IProgram.State
        { 
            get 
            {
                return (ProgramState)Enum.Parse(typeof(ProgramState), State, true);
            } 
        }

        /// <summary>
        /// Gets or sets the length of the DVR window.
        /// </summary>
        public TimeSpan ArchiveWindowLength { get; set; }

        /// <summary>
        /// Gets the channel associated with the program.
        /// </summary>
        IChannel IProgram.Channel
        {
            get
            {
                if ((_channel == null) && !String.IsNullOrWhiteSpace(ChannelId))
                {
                    _channel = GetMediaContext().Channels.Where(c => c.Id == ChannelId).Single();
                }

                return _channel;
            }
        }

        /// <summary>
        /// Gets the asset associated with the program.
        /// </summary>
        IAsset IProgram.Asset
        {
            get
            {
                if ((_asset == null) && !String.IsNullOrWhiteSpace(AssetId))
                {
                    _asset = GetMediaContext().Assets.Where(a => a.Id == AssetId).Single();
                }

                return _asset;
            }
        }

        /// <summary>
        /// Starts the program.
        /// </summary>
        public void Start()
        {
            AsyncHelper.Wait(StartAsync());
        }

        /// <summary>
        /// Starts the program asynchronously.
        /// </summary>
        /// <returns>Task to wait on for operation completion.</returns>
        public Task StartAsync()
        {
            var uri = new Uri(string.Format(CultureInfo.InvariantCulture, "/Programs('{0}')/Start", Id), UriKind.Relative);

            return ExecuteActionAsync(uri, StreamingConstants.StartProgramPollInterval);
        }

        /// <summary>
        /// Sends start operation to the service and returns. Use Operations collection to get operation's status.
        /// </summary>
        /// <returns>Operation info that can be used to track the operation.</returns>
        public IOperation SendStartOperation()
        {
            var uri = new Uri(string.Format(CultureInfo.InvariantCulture, "/Programs('{0}')/Start", Id), UriKind.Relative);

            return SendOperation(uri);
        }

        /// <summary>
        /// Sends start operation to the service asynchronously. Use Operations collection to get operation's status.
        /// </summary>
        /// <returns>Task to wait on for operation sending completion.</returns>
        public Task<IOperation> SendStartOperationAsync()
        {
            return Task.Factory.StartNew(() => SendStartOperation());
        }

        /// <summary>
        /// Stops the program.
        /// </summary>
        public void Stop()
        {
            AsyncHelper.Wait(StopAsync());
        }

        /// <summary>
        /// Stops the program asynchronously.
        /// </summary>
        /// <returns>Task to wait on for operation completion.</returns>
        public Task StopAsync()
        {
            var uri = new Uri(string.Format(CultureInfo.InvariantCulture, "/Programs('{0}')/Stop", Id), UriKind.Relative);

            return ExecuteActionAsync(uri, StreamingConstants.StopProgramPollInterval);
        }

        /// <summary>
        /// Sends stop operation to the service and returns. Use Operations collection to get operation's status.
        /// </summary>
        /// <returns>Operation info that can be used to track the operation.</returns>
        public IOperation SendStopOperation()
        {
            var uri = new Uri(string.Format(CultureInfo.InvariantCulture, "/Programs('{0}')/Stop", Id), UriKind.Relative);

            return SendOperation(uri);
        }

        /// <summary>
        /// Sends stop operation to the service asynchronously. Use Operations collection to get operation's status.
        /// </summary>
        /// <returns>Task to wait on for operation sending completion.</returns>
        public Task<IOperation> SendStopOperationAsync()
        {
            return Task.Factory.StartNew(() => SendStopOperation());
        }
    }
}
