using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Microsoft.Win32;
using WordCounter.Models;
using WordCounter.Models.CountStrategies;
using WordCounter.Models.Interfaces;
using WordCounter.Models.Readers;
using WordCounter.UI.Infrastructure;

namespace WordCounter.UI
{
    internal sealed class MainViewModel : INotifyPropertyChanged
    {
        private readonly Models.WordCounter _counter;
        private readonly RelayCommand _countWordsCommand;
        private readonly List<SelectionWrapper<ICountStrategy>> _strategies;
        private string _pathToFile;
        private IEnumerable<WordResult> _results;
        private IEnumerable<string> _infoResults;
        private bool _isShowMemory;
        private bool _isShowSpeed;

        public MainViewModel()
        {
            Results = new WordResult[0];
            _strategies = new List<SelectionWrapper<ICountStrategy>>();
            _infoResults = new List<string>();

            ChooseFileCommand = new RelayCommand(OnChooseFile);
            GCCleanCommand = new RelayCommand(obj => GC.Collect());
            _countWordsCommand = new RelayCommand(OnCountWords,
                                         obj => TextSourceFactory.Instance.IsFileSupported(PathToFile));

            var defaultStrategy = new CountAllWords();
            _strategies.Add(new SelectionWrapper<ICountStrategy>(defaultStrategy) { IsSelected = true });
            _strategies.Add(new SelectionWrapper<ICountStrategy>(new BoyerMooreWordCountStrategy()));
            _strategies.Add(new SelectionWrapper<ICountStrategy>(new SortCountStrategy()));

            _counter = new Models.WordCounter(defaultStrategy);
        }


        /// <summary>
        /// Gets or sets a value indicating whether this instance is show memory info.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is show memory info; otherwise, <c>false</c>.
        /// </value>
        public bool IsShowMemory
        {
            get
            {
                return _isShowMemory;
            }
            set
            {
                _isShowMemory = value;
                OnPropertyChanged("IsShowInfo");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is show speed info.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is show speed info; otherwise, <c>false</c>.
        /// </value>
        public bool IsShowSpeed
        {
            get
            {
                return _isShowSpeed;
            }
            set
            {
                _isShowSpeed = value;
                OnPropertyChanged("IsShowInfo");
            }
        }

        /// <summary>
        /// Gets or sets the path to file.
        /// </summary>
        /// <value>
        /// The path to file.
        /// </value>
        public string PathToFile
        {
            get
            {
                return _pathToFile;
            }
            set
            {
                _pathToFile = value;
                _countWordsCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("PathToFile");
            }
        }

        /// <summary>
        /// Gets command used to invoke words count.
        /// </summary>
        /// <value>
        /// The count words.
        /// </value>
        public ICommand CountWordsCommand { get { return _countWordsCommand; } }

        /// <summary>
        /// Gets the results of count.
        /// </summary>
        /// <value>
        /// The results.
        /// </value>
        public IEnumerable<WordResult> Results
        {
            get
            {
                return _results;
            }
            private set
            {
                _results = value;
                OnPropertyChanged("Results");
            }
        }

        /// <summary>
        /// Gets the info results.
        /// </summary>
        /// <value>
        /// The info results.
        /// </value>
        public IEnumerable<string> InfoResults
        {
            get
            {
                return _infoResults;
            }
            private set
            {
                _infoResults = value;
                OnPropertyChanged("InfoResults");
            }
        }

        /// <summary>
        /// Gets the choose file command.
        /// </summary>
        /// <value>
        /// The choose file command.
        /// </value>
        public ICommand ChooseFileCommand { get; private set; }

        /// <summary>
        /// Gets the strategies.
        /// </summary>
        /// <value>
        /// The strategies.
        /// </value>
        public IEnumerable<SelectionWrapper<ICountStrategy>> Strategies
        {
            get
            {
                return _strategies;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is need to show info.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is show info; otherwise, <c>false</c>.
        /// </value>
        public bool IsShowInfo
        {
            get
            {
                return IsShowMemory || IsShowSpeed;
            }
        }

        /// <summary>
        /// Gets or sets the limit result to.
        /// </summary>
        /// <value>
        /// The limit result to.
        /// </value>
        public string LimitResultTo { get; set; }

        /// <summary>
        /// Gets the GC clean command.
        /// </summary>
        /// <value>
        /// The GC clean command.
        /// </value>
        public ICommand GCCleanCommand { get; private set; }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnCountWords(object obj)
        {
            var strategy = Strategies.First(item => item.IsSelected).Content;

            if (IsShowSpeed)
            {
                strategy = new SpeedCountStrategyDecorator(strategy);
            }

            if (IsShowMemory)
            {
                strategy = new MemoryUsageCountStrategyDecorator(strategy);
            }

            _counter.SetSearchStrategy(strategy);
            int limitTo;
            int.TryParse(LimitResultTo, out limitTo);
            Results = _counter.CountWords(PathToFile, limitTo);

            var strategyDecorator = strategy as CountStrategyDecoratorBase;
            if (IsShowMemory || IsShowSpeed)
            {
                var infoResults = new List<string>();
                while (strategyDecorator != null)
                {
                    infoResults.AddRange(strategyDecorator.LogResults);
                    strategyDecorator = strategyDecorator.DecoratedCountStrategy as CountStrategyDecoratorBase;
                }
                InfoResults = infoResults;
            }

        }

        private void OnChooseFile(object obj)
        {
            var dlg = new OpenFileDialog();

            var builder = new StringBuilder("TextFormats");
            foreach (var textFormat in TextSourceFactory.Instance.SupportedTextFormats)
            {
                builder.Append(string.Format(" (*{0})|*{0}", textFormat));
            }

            dlg.Filter = builder.ToString();

            if ((bool)dlg.ShowDialog())
            {
                PathToFile = dlg.FileName;
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
